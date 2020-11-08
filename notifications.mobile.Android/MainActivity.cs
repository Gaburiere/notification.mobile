using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using notification.mobile.Android.Services;
using notification.mobile.core.Services;
using Xamarin.Forms;

namespace notification.mobile.Android
{
    [Activity(Label = "notifications.mobile", Theme = "@style/MainTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            //registers the INotificationManager interface implementation with the DependencyService.
            DependencyService.Register<INotificationManager, AndroidNotificationManager>();
            
            base.OnCreate(savedInstanceState);
            Forms.Init(this, savedInstanceState);
            this.LoadApplication(new App());

            //When the application is started by notification data, the Intent data will be passed to the OnCreate method.
            this.CreateNotificationFromIntent(this.Intent);
        }
        
        protected override void OnNewIntent(Intent intent)
        {
            //If the application is already in the foreground, the Intent data will be passed to the OnNewIntent method.
            this.CreateNotificationFromIntent(intent);
        }

        /// <summary>
        /// extracts notification data from the intent argument and provides it to the AndroidNotificationManager using the ReceiveNotification method.
        /// </summary>
        /// <param name="intent"></param>
        private void CreateNotificationFromIntent(Intent intent)
        {
            if (intent?.Extras != null)
            {
                var title = intent.Extras.GetString(AndroidNotificationManager.TitleKey);
                var message = intent.Extras.GetString(AndroidNotificationManager.MessageKey);
                DependencyService.Get<INotificationManager>().ReceiveNotification(title, message);
            }
        }
    }
}