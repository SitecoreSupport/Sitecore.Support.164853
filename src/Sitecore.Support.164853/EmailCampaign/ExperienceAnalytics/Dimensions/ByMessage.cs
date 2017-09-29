namespace Sitecore.Support.EmailCampaign.ExperienceAnalytics.Dimensions
{
    using System;
    using Sitecore.EmailCampaign.ExperienceAnalytics;
    using Sitecore.Support.EmailCampaign.ExperienceAnalytics.Properties;
    using Sitecore.ExM.Framework.Diagnostics;

    internal class ByMessage : ExmDimensionBase
    {
        public ByMessage(Guid dimensionId)
            : base(dimensionId, true, true)
        {
        }

        internal ByMessage(ILogger logger, Guid dimensionId)
            : base(logger, dimensionId, true, true)
        {
        }

        internal override string GenerateCustomKey(VisitAggregationState visitState)
        {
            if (visitState.CustomValues == null)
            {
                Logger.LogDebug(string.Format(Settings.Default.VisitAggregationStateParameterIsNullOrEmptyMessagePattern,
                    "CustomValues", GetType().Name));
                return null;
            }


            if (visitState.CustomValues.MessageLanguage == null)
            {
                Logger.LogDebug(string.Format(Settings.Default.VisitAggregationStateParameterIsNullOrEmptyMessagePattern,
                    "CustomValues.MessageLanguage", GetType().Name));
                return null;
            }

            return new KeyBuilder()
                .Add(((int)visitState.PageEvent).ToString())
                .Add(visitState.CustomValues.MessageLanguage)
                .Add(visitState.IsProductive)
                .Add(visitState.IsBrowsed)
                .ToString();
        }
    }
}
