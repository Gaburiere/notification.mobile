namespace notification.mobile.Features.PushNotifications
{
    public partial class PushNotificationPage
    {
        public PushNotificationPage()
        {
            this.InitializeComponent();
            this.BindingContext = new PushNotificationViewModel();
        }
    }
}