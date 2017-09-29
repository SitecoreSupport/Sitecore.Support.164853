namespace Sitecore.Support.EmailCampaign.ExperienceAnalytics.Dimensions
{
    using Sitecore.EmailCampaign.ExperienceAnalytics;
    using Sitecore.EmailCampaign.ExperienceAnalytics.Dimensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Sitecore.Analytics.Aggregation.Data.Model;
    using Sitecore.Analytics.Model;
    using Sitecore.Diagnostics;
    using Sitecore.Support.EmailCampaign.ExperienceAnalytics.Properties;
    using Sitecore.ExM.Framework.Diagnostics;
    using Sitecore.ExperienceAnalytics.Aggregation;
    using Sitecore.ExperienceAnalytics.Aggregation.Data.Model;
    using Sitecore.ExperienceAnalytics.Aggregation.Data.Schema;
    using Sitecore.ExperienceAnalytics.Aggregation.Dimensions;
    using Sitecore.ExperienceAnalytics.Core;

    public abstract class ExmDimensionBase : DimensionBase
    {
        private readonly ExmExpert _exmExpert;
        private readonly bool _processClickEvents;
        private readonly bool _processOpenEvents;

        internal readonly ILogger Logger;

        protected ExmDimensionBase(Guid dimensionId, bool processClickEvents, bool processOpenEvents)
            : this(ExM.Framework.Diagnostics.Logger.Instance, dimensionId, processClickEvents, processOpenEvents)
        {
        }

        protected ExmDimensionBase([NotNull] ILogger logger, Guid dimensionId, bool processClickEvents, bool processOpenEvents)
            : base(dimensionId)
        {
            Assert.ArgumentNotNull(logger, "logger");

            Logger = logger;
            _processClickEvents = processClickEvents;
            _processOpenEvents = processOpenEvents;
            _exmExpert = new ExmExpert();
        }

        public override IEnumerable<DimensionData> GetData([NotNull] IVisitAggregationContext context)
        {
            Assert.ArgumentNotNull(context, "context");

            var visitState = new VisitAggregationState
            {
                VisitContext = context
            };

            var dimensions = new List<DimensionData>();

            var visit = context.Visit;

            if (visit == null)
            {
                return dimensions;
            }

            if (_processClickEvents)
            {
                if (visit.CustomValues != null && visit.CustomValues.Any())
                {
                    dimensions = GetClickEventData(visit, visitState).ToList();
                }
            }

            if (_processOpenEvents)
            {
                var openData = GetOpenEventData(visit, visitState);
                if (openData != null)
                {
                    dimensions.Add(openData);
                }
            }

            return dimensions;
        }

        protected internal virtual DimensionData GetOpenEventData([NotNull] VisitData visit, [NotNull] VisitAggregationState visitState)
        {
            Assert.ArgumentNotNull(visit, "visit");
            Assert.ArgumentNotNull(visitState, "visitState");

            if (visit.Pages == null || !visit.Pages.Any())
            {
                return null;
            }

            visitState.CustomValues = DimensionUtils.GetPageCustomValues(visit);

            var baseKey = GenerateBaseKey(visitState);
            if (baseKey == null)
            {
                return null;
            }

            var dimension = new DimensionData
            {
                MetricsValue = new SegmentMetricsValue
                {
                    Visits = 1,
                    Pageviews = 1
                }
            };

            visitState.LandingPage = visit.Pages.First();

            foreach (var pageEvent in visitState.LandingPage.PageEvents)
            {
                var pageEventType = DimensionUtils.ParsePageEvent(pageEvent.PageEventDefinitionId);

                if (pageEventType == ExmPageEventType.Bounce || pageEventType == ExmPageEventType.Sent || pageEventType == ExmPageEventType.Spam)
                {
                    visitState.PageEvent = pageEventType;
                    break;
                }

                if (pageEventType == ExmPageEventType.Open)
                {
                    visitState.PageEvent = pageEventType;
                }
                else if (pageEventType == ExmPageEventType.FirstOpen)
                {
                    visitState.PageEvent = ExmPageEventType.Open;
                    dimension.MetricsValue.Count = 1;
                    break;
                }
            }

            if (visitState.PageEvent != ExmPageEventType.Unspecified)
            {
                var customKey = GenerateCustomKey(visitState);
                if (customKey == null)
                {
                    return null;
                }

                dimension.DimensionKey = string.Format("{0}_{1}", GenerateBaseKey(visitState), customKey);

                return dimension;
            }

            return null;
        }

        protected internal virtual IEnumerable<DimensionData> GetClickEventData([NotNull] VisitData visit, [NotNull] VisitAggregationState visitState)
        {
            Assert.ArgumentNotNull(visit, "visit");
            Assert.ArgumentNotNull(visitState, "visitState");

            var customValuesHolder = _exmExpert.GetEmailFingerPrints(visit);

            var dimensions = new Dictionary<ExmPageEventType, string>();

            if (customValuesHolder != null && customValuesHolder.ExmCustomValues != null)
            {
                var indexes = customValuesHolder.ExmCustomValues.Keys.ToArray();

                for (var i = 0; i < indexes.Length; i++)
                {
                    var firstPageIndex = indexes[i] - 1;
                    var lastPageIndex = i == indexes.Length - 1 ? visit.Pages.Count - 1 : indexes[i + 1] - 2;

                    visitState.CustomValues = customValuesHolder.ExmCustomValues[indexes[i]];

                    var baseKey = GenerateBaseKey(visitState);
                    if (baseKey != null)
                    {
                        visitState.LandingPage = visit.Pages[firstPageIndex];

                        var dimensionData = GenerateDimension(visit, visitState, firstPageIndex, lastPageIndex, baseKey);

                        if (dimensionData != null && !dimensions.Any(d => d.Key == ExmPageEventType.Unsubscribe && d.Value == dimensionData.DimensionKey))
                        {
                            dimensions[visitState.PageEvent] = dimensionData.DimensionKey;
                            yield return dimensionData;
                        }
                    }
                }
            }
        }

        private DimensionData GenerateDimension(VisitData visit, VisitAggregationState visitState, int firstPageIndex, int lastPageIndex, string baseKey)
        {
            DimensionData unsubscribeDimension = null;

            var clickDimension = new DimensionData
            {
                MetricsValue = new SegmentMetricsValue
                {
                    Visits = 1,
                    Pageviews = lastPageIndex - firstPageIndex + 1
                }
            };

            for (var i = firstPageIndex; i <= lastPageIndex; i++)
            {
                foreach (var pageEvent in visit.Pages[i].PageEvents)
                {
                    var pageEventType = DimensionUtils.ParsePageEvent(pageEvent.PageEventDefinitionId);
                    if (pageEventType != ExmPageEventType.Unspecified)
                    {
                        if (pageEventType == ExmPageEventType.Unsubscribe)
                        {
                            unsubscribeDimension = new DimensionData
                            {
                                MetricsValue = new SegmentMetricsValue
                                {
                                    Visits = 1,
                                    Pageviews = 1
                                }
                            };
                        }

                        if (pageEventType == ExmPageEventType.FirstClick)
                        {
                            clickDimension.MetricsValue.Count = 1;
                        }
                    }

                    if (pageEvent.IsGoal)
                    {
                        clickDimension.MetricsValue.Conversions++;
                    }

                    clickDimension.MetricsValue.Value += pageEvent.Value;
                }

                clickDimension.MetricsValue.TimeOnSite += visit.Pages[i].Duration;
            }

            DimensionData dimension;

            if (unsubscribeDimension == null)
            {
                dimension = clickDimension;

                visitState.PageEvent = ExmPageEventType.Click;
                visitState.IsProductive = clickDimension.MetricsValue.Value > 0;
                visitState.IsBrowsed = clickDimension.MetricsValue.Pageviews > 1;
            }
            else
            {
                dimension = unsubscribeDimension;

                visitState.PageEvent = ExmPageEventType.Unsubscribe;
            }

            clickDimension.MetricsValue.Bounces = visitState.IsBrowsed ? 0 : 1;

            var customKey = GenerateCustomKey(visitState);
            if (customKey == null)
            {
                return null;
            }

            dimension.DimensionKey = string.Format("{0}_{1}", baseKey, customKey);
            return dimension;
        }

        internal virtual string GetGroupResolverValue([NotNull] string groupName, [NotNull] IVisitAggregationContext context)
        {
            Assert.ArgumentNotNull(groupName, "groupName");
            Assert.ArgumentNotNull(context, "context");

            try
            {
                if (!ValidateVisitForResolver(context.Visit))
                {
                    return null;
                }

                var resolver = AggregationContainer.GetGroupResolver(groupName);
                return resolver.GetGroupIds(context).First();
            }
            catch (Exception ex)
            {
                Logger.LogDebug(string.Format("{0}, {1}", Settings.Default.GetGroupResolverValueExceptionMessage, ex));
                return null;
            }
        }

        internal string GenerateBaseKey([NotNull] VisitAggregationState visitState)
        {
            if (visitState.CustomValues == null)
            {
                Logger.LogDebug(string.Format(Settings.Default.VisitAggregationStateParameterIsNullOrEmptyMessagePattern,
                    "CustomValues", GetType().Name));
                return null;
            }

            if (visitState.CustomValues.ManagerRootId == default(Guid))
            {
                Logger.LogDebug(string.Format(Settings.Default.VisitAggregationStateParameterIsNullOrEmptyMessagePattern,
                    "CustomValues.ManagerRootId", GetType().Name));
                return null;
            }

            if (visitState.CustomValues.MessageId == default(Guid))
            {
                Logger.LogDebug(string.Format(Settings.Default.VisitAggregationStateParameterIsNullOrEmptyMessagePattern,
                    "CustomValues.MessageId", GetType().Name));
                return null;
            }

            return
                new HierarchicalKeyBuilder()
                    .Add(visitState.CustomValues.ManagerRootId)
                    .Add(visitState.CustomValues.MessageId)
                    .ToString();
        }

        internal abstract string GenerateCustomKey([NotNull] VisitAggregationState visitState);

        internal virtual bool ValidateVisitForResolver([NotNull] VisitData visit)
        {
            return true;
        }
    }
}
