namespace Sitecore.Support.EmailCampaign.ExperienceAnalytics.Dimensions
{
    using Sitecore.Analytics.Model;
    using Sitecore.EmailCampaign.Analytics.Model;

    internal class ExmExpert
    {
        public ExmCustomValuesHolder GetEmailFingerPrints(VisitData visit)
        {
            var emailFingerPrints = ExmCustomValuesHolder.GetValueHolder(visit.CustomValues);

            if (emailFingerPrints == null)
            {
                return null;
            }

            return emailFingerPrints.ExmCustomValues.Count <= 0 ? null : emailFingerPrints;
        }
    }
}
