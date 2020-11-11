using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.OS;
using Android.Util;
using notification.mobile.Android.Services;
using notification.mobile.core.Classes;
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
            // this.CreateNotificationFromIntent(this.Intent);
            if (this.IsPlayServiceAvailable())
            {
                // DependencyService.Resolve<INotificationManager>().Initialize();
                CreateNotificationChannel();
            }
            
        }
        void CreateNotificationChannel()
        {
            // Notification channels are new as of "Oreo".
            // There is no need to create a notification channel on older versions of Android.
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelName = AppConstants.NotificationChannelName;
                var channelDescription = string.Empty;
                var channel = new NotificationChannel(channelName, channelName, NotificationImportance.Default)
                {
                    Description = channelDescription
                };

                var notificationManager = (NotificationManager) this.GetSystemService(NotificationService);
                notificationManager.CreateNotificationChannel(channel);
            }
        }
        
        bool IsPlayServiceAvailable()
        {
            var resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    Log.Debug(AppConstants.DebugTag, GoogleApiAvailability.Instance.GetErrorString(resultCode));
                else
                {
                    Log.Debug(AppConstants.DebugTag, "This device is not supported");
                }
                return false;
            }
            return true;
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