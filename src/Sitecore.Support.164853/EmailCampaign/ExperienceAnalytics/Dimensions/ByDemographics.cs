namespace Sitecore.Support.EmailCampaign.ExperienceAnalytics.Dimensions
{
    using Sitecore.EmailCampaign.ExperienceAnalytics;
    using Sitecore.EmailCampaign.ExperienceAnalytics.Dimensions;
    using System;
    using Sitecore.Analytics.Model.Entities;
    using Sitecore.Analytics.Model.Framework;
    using Sitecore.Support.EmailCampaign.ExperienceAnalytics.Properties;
    using Sitecore.ExM.Framework.Diagnostics;

    internal class ByDemographics : ExmDimensionBase
    {
        public ByDemographics(Guid dimensionId)
          : base(dimensionId, true, true)
        {
        }

        internal ByDemographics(ILogger logger, Guid dimensionId)
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

            try
            {
                var personal = visitState.VisitContext.Contact.GetFacet<IContactPersonalInfo>("Personal");

                var gender = personal.Gender ?? Settings.Default.UnspecifiedValue;
                var age = personal.BirthDate.HasValue ? DimensionUtils.CalculateAge(personal.BirthDate.Value, DateTime.UtcNow) : 0;

                if (gender == Settings.Default.UnspecifiedValue && age == 0)
                {
                    return null;
                }

                return new KeyBuilder()
                .Add((int)visitState.PageEvent)
                .Add(gender)
                .Add(age)
                .ToString();
            }
            catch (FacetNotAvailableException)
            {
                Logger.LogDebug(string.Format(Settings.Default.FacetIsNotAvailableMessagePattern,
                  "Personal", this.GetType().Name));
                return null;
            }
        }
    }
}
