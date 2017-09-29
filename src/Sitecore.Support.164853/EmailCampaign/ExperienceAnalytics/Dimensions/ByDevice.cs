namespace Sitecore.Support.EmailCampaign.ExperienceAnalytics.Dimensions
{
    using System;
    using Sitecore.Analytics.Model;
    using Sitecore.ExM.Framework.Diagnostics;
    using Sitecore.EmailCampaign.ExperienceAnalytics;

    internal class ByDevice : ExmDimensionBase
    {

        public ByDevice(Guid dimensionId)
          : base(dimensionId, true, true)
        {
        }

        internal ByDevice(ILogger logger, Guid dimensionId)
          : base(logger, dimensionId, true, true)
        {
        }

        internal override string GenerateCustomKey(VisitAggregationState visitState)
        {
            var device = GetGroupResolverValue("DeviceType", visitState.VisitContext);
            var model = GetGroupResolverValue("DeviceModel", visitState.VisitContext);
            var browser = GetGroupResolverValue("BrowserModel", visitState.VisitContext);
            var os = GetGroupResolverValue("OperationSystem", visitState.VisitContext);

            if (device == null || model == null || browser == null || os == null)
            {
                return null;
            }

            return new KeyBuilder()
              .Add(((int)visitState.PageEvent).ToString())
              .Add(device)
              .Add(model)
              .Add(browser)
              .Add(os)
              .ToString();
        }

        internal override bool ValidateVisitForResolver(VisitData visit)
        {
            return visit.UserAgent != null;
        }
    }
}
