namespace Sitecore.Support.EmailCampaign.ExperienceAnalytics.Dimensions
{
    using System;
    using Sitecore.Analytics.Model;
    using Sitecore.EmailCampaign.ExperienceAnalytics;
    using Sitecore.ExM.Framework.Diagnostics;

    internal class ByLocation : ExmDimensionBase
    {

        public ByLocation(Guid dimensionId)
          : base(dimensionId, true, true)
        {
        }

        internal ByLocation(ILogger logger, Guid dimensionId)
          : base(logger, dimensionId, true, true)
        {
        }

        internal override string GenerateCustomKey(VisitAggregationState visitState)
        {
            var country = GetGroupResolverValue("Country", visitState.VisitContext);
            var region = GetGroupResolverValue("Region", visitState.VisitContext);
            var city = GetGroupResolverValue("City", visitState.VisitContext);

            if (country == null || region == null || city == null)
            {
                return null;
            }

            return new KeyBuilder()
              .Add(((int)visitState.PageEvent).ToString())
              .Add(country)
              .Add(region)
              .Add(city)
              .ToString();
        }

        internal override bool ValidateVisitForResolver(VisitData visit)
        {
            return visit.GeoData != null;
        }
    }
}
