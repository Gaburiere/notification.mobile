namespace notification.mobile.core.Classes
{
    public static class AppConstants
    {
        public static string NotificationChannelName { get; set; } = "XamarinNotifyChannel";
        
        /// <summary>
        /// the name of the Azure Notification Hub you created in your Azure portal.
        /// </summary>
        public static string NotificationHubName { get; set; } = "formazionenhub";
        
        /// <summary>
        /// This value is found in the Azure Notification Hub under Access Policies.
        /// </summary>
        public static string ListenConnectionString { get; set; } = 
            "Endpoint=sb://formazionenhubns.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=0M+ggpBddNkRSNr9e4S57k/ImlaiJsi80diYSpylWgE=";
        public static string DebugTag { get; set; } = "XamarinNotify";
        public static string[] SubscriptionTags { get; set; } = { "default" };
        public static string FCMTemplateBody { get; set; } = "{\"data\":{\"message\":\"$(messageParam)\"}}";
        public static string APNTemplateBody { get; set; } = "{\"aps\":{\"alert\":\"$(messageParam)\"}}";
        
        public static string TitleKey = "title";
        public static string MessageKey = "message";
        public static string TypeKey = "type";
    }
}