namespace notification.mobile.Features.LocalNotifications
{
    public partial class LocalNotificationPage
    {
        public LocalNotificationPage()
        {
            this.InitializeComponent();
            this.BindingContext = new LocalNotificationViewModel();
        }
    }
}