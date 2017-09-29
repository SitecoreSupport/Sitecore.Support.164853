namespace Sitecore.Support.EmailCampaign.ExperienceAnalytics.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.Configuration;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0"), CompilerGenerated]
    internal sealed class Settings : ApplicationSettingsBase
    {
        private static Settings defaultInstance = ((Settings)SettingsBase.Synchronized(new Settings()));

        [DefaultSettingValue("~/icon/office/16x16/mail_exchange.png"), ApplicationScopedSetting, DebuggerNonUserCode]
        public string BounceEventImagePath =>
            ((string)this["BounceEventImagePath"]);

        [DefaultSettingValue("f7e054f5-6f73-4c09-82b0-9f36141be42f"), ApplicationScopedSetting, DebuggerNonUserCode]
        public Guid BounceEventItemId =>
            ((Guid)this["BounceEventItemId"]);

        [DefaultSettingValue("0ec316ba-73e7-4c72-9c7d-43a711c11bc9"), ApplicationScopedSetting, DebuggerNonUserCode]
        public Guid ByAbTestVariantSegmentId =>
            ((Guid)this["ByAbTestVariantSegmentId"]);

        [ApplicationScopedSetting, DefaultSettingValue("c1745f34-f2b9-4ac3-a6de-faee8ce62ae1"), DebuggerNonUserCode]
        public Guid ByLandingPageSegmentId =>
            ((Guid)this["ByLandingPageSegmentId"]);

        [DefaultSettingValue("399d686d-16b6-46e3-89e9-44fb9535c2b2"), DebuggerNonUserCode, ApplicationScopedSetting]
        public Guid ByTimeOfDaySegmentId =>
            ((Guid)this["ByTimeOfDaySegmentId"]);

        [DebuggerNonUserCode, ApplicationScopedSetting, DefaultSettingValue("~/icon/office/16x16/mouse_pointer.png")]
        public string ClickEventImagePath =>
            ((string)this["ClickEventImagePath"]);

        [ApplicationScopedSetting, DebuggerNonUserCode, DefaultSettingValue("87431b9b-fa39-4780-beb3-1047b9e61876")]
        public Guid ClickEventItemId =>
            ((Guid)this["ClickEventItemId"]);

        public static Settings Default =>
            defaultInstance;

        [SettingsDescription("A message pattern for logging a debug message that a dimension will not be processed because the facet is not available: {0} - facet name; {1} - dimension name."), ApplicationScopedSetting, DebuggerNonUserCode, DefaultSettingValue("The '{0}' facet is not available and the '{1}' dimension will not be processed!")]
        public string FacetIsNotAvailableMessagePattern =>
            ((string)this["FacetIsNotAvailableMessagePattern"]);

        [DebuggerNonUserCode, ApplicationScopedSetting, DefaultSettingValue("bfc9eb31-1d02-486b-a3a0-5b36a138ccf7")]
        public Guid FirstClickEventItemId =>
            ((Guid)this["FirstClickEventItemId"]);

        [DefaultSettingValue("e97e9557-0b84-4103-b545-988bf7336c7c"), ApplicationScopedSetting, DebuggerNonUserCode]
        public Guid FirstOpenEventItemId =>
            ((Guid)this["FirstOpenEventItemId"]);

        [ApplicationScopedSetting, DebuggerNonUserCode, DefaultSettingValue("ExmDimensionBase: An exception occurred when getting the Group Resolver value!")]
        public string GetGroupResolverValueExceptionMessage =>
            ((string)this["GetGroupResolverValueExceptionMessage"]);

        [ApplicationScopedSetting, DefaultSettingValue("~/icon/office/16x16/mail_open2.png"), DebuggerNonUserCode]
        public string OpenEventImagePath =>
            ((string)this["OpenEventImagePath"]);

        [DebuggerNonUserCode, ApplicationScopedSetting, DefaultSettingValue("1ad8ebeb-24e3-46f1-9edd-2295c5219c5e")]
        public Guid OpenEventItemId =>
            ((Guid)this["OpenEventItemId"]);

        [DebuggerNonUserCode, DefaultSettingValue("2a65acc5-9851-40dd-851b-23f7a6c53092"), ApplicationScopedSetting]
        public Guid SentEventItemId =>
            ((Guid)this["SentEventItemId"]);

        [ApplicationScopedSetting, DebuggerNonUserCode, DefaultSettingValue("~/icon/office/16x16/mail_bug.png")]
        public string SpamEventImagePath =>
            ((string)this["SpamEventImagePath"]);

        [DefaultSettingValue("d5ab8d8d-efc1-4eec-b7f1-80cdd05febd3"), DebuggerNonUserCode, ApplicationScopedSetting]
        public Guid SpamEventItemId =>
            ((Guid)this["SpamEventItemId"]);

        [DebuggerNonUserCode, ApplicationScopedSetting, DefaultSettingValue("unspecified")]
        public string UnspecifiedValue =>
            ((string)this["UnspecifiedValue"]);

        [DebuggerNonUserCode, DefaultSettingValue("450adcbf-9429-48d1-b87f-b45691833d1f"), ApplicationScopedSetting]
        public Guid UnsubscribeEventItemId =>
            ((Guid)this["UnsubscribeEventItemId"]);

        [ApplicationScopedSetting, DefaultSettingValue("Parameter '{0}' of VisitAggregationState is null or empty and the '{1}' dimension will not be processed!"), SettingsDescription("A message pattern for logging a debug message that a dimension will not be processed because parameters are null or empty: {0} - parameter name; {1} - dimension name."), DebuggerNonUserCode]
        public string VisitAggregationStateParameterIsNullOrEmptyMessagePattern =>
            ((string)this["VisitAggregationStateParameterIsNullOrEmptyMessagePattern"]);
    }
}
