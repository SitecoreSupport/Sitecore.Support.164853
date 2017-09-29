namespace Sitecore.Support.EmailCampaign.ExperienceAnalytics.Dimensions
{
    using System;
    using Sitecore.Analytics.Model.Entities;
    using Sitecore.Analytics.Model.Framework;
    using Sitecore.EmailCampaign.ExperienceAnalytics;
    using Sitecore.Support.EmailCampaign.ExperienceAnalytics.Properties;
    using Sitecore.ExM.Framework.Diagnostics;

    internal class ByTimeOfDay : ExmDimensionBase
    {

        public ByTimeOfDay(Guid dimensionId)
            : base(dimensionId, true, true)
        {
        }

        internal ByTimeOfDay(ILogger logger, Guid dimensionId)
            : base(logger, dimensionId, true, true)
        {
        }

        internal override string GenerateCustomKey(VisitAggregationState visitState)
        {
            if (visitState.VisitContext == null)
            {
                Logger.LogDebug(string.Format(Settings.Default.VisitAggregationStateParameterIsNullOrEmptyMessagePattern,
                    "VisitContext", this.GetType().Name));
                return null;
            }

            if (visitState.VisitContext.Visit == null)
            {
                Logger.LogDebug(string.Format(Settings.Default.VisitAggregationStateParameterIsNullOrEmptyMessagePattern,
                    "VisitContext.Visit", this.GetType().Name));
                return null;
            }

            var dayOfWeek = (int)visitState.VisitContext.Visit.StartDateTime.DayOfWeek;
            var hour = visitState.VisitContext.Visit.StartDateTime.Hour;

            return new KeyBuilder()
                      .Add((int)visitState.PageEvent)
                      .Add(dayOfWeek)
                      .Add(hour)
                      .ToString();
        }
    }
}