namespace Sitecore.Support.EmailCampaign.ExperienceAnalytics.Dimensions
{
    using Sitecore.EmailCampaign.ExperienceAnalytics;
    using System;
    using Sitecore.Support.EmailCampaign.ExperienceAnalytics.Properties;
    using Sitecore.ExM.Framework.Diagnostics;

    internal class ByLandingPage : ExmDimensionBase
    {
        public ByLandingPage(Guid dimensionId)
            : base(dimensionId, true, false)
        {
        }

        internal ByLandingPage(ILogger logger, Guid dimensionId)
            : base(logger, dimensionId, true, true)
        {
        }

        internal override string GenerateCustomKey(VisitAggregationState visitState)
        {
            if (visitState.LandingPage == null)
            {
                Logger.LogDebug(string.Format(Settings.Default.VisitAggregationStateParameterIsNullOrEmptyMessagePattern,
                    "LandingPage", GetType().Name));
                return null;
            }

            if (visitState.LandingPage.Url.Path != null &&
                visitState.LandingPage.Item.Id != Guid.Empty &&
                visitState.PageEvent != ExmPageEventType.Unsubscribe)
            {
                return new KeyBuilder()
                    .Add(visitState.LandingPage.Url.Path)
                    .Add(visitState.LandingPage.Item.Id)
                    .ToString();
            }

            return null;
        }
    }
}
