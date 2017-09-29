namespace Sitecore.Support.EmailCampaign.ExperienceAnalytics.Dimensions
{
    using System;
    using Sitecore.EmailCampaign.ExperienceAnalytics;
    using Sitecore.Support.EmailCampaign.ExperienceAnalytics.Properties;
    using Sitecore.ExM.Framework.Diagnostics;

    internal class ByAbTestVariant : ExmDimensionBase
    {

        public ByAbTestVariant(Guid dimensionId)
          : base(dimensionId, true, true)
        {
        }

        internal ByAbTestVariant(ILogger logger, Guid dimensionId)
          : base(logger, dimensionId, true, true)
        {
        }

        internal override string GenerateCustomKey(VisitAggregationState visitState)
        {
            if (visitState.CustomValues == null)
            {
                Logger.LogDebug(string.Format(Settings.Default.VisitAggregationStateParameterIsNullOrEmptyMessagePattern,
                  "CustomValues", this.GetType().Name));
                return null;
            }

            if (!visitState.CustomValues.TestValueIndex.HasValue)
            {
                Logger.LogDebug(string.Format(Settings.Default.VisitAggregationStateParameterIsNullOrEmptyMessagePattern,
                  "CustomValues.TestValueIndex", this.GetType().Name));
                return null;
            }

            return new KeyBuilder()
              .Add(((int)visitState.PageEvent).ToString())
              .Add(visitState.CustomValues.TestValueIndex.Value.ToString())
              .ToString();
        }
    }
}
