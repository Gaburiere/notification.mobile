using System;
using System.Collections.Generic;
using System.Linq;
using WindowsAzure.Messaging;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using Firebase.Messaging;
using notification.mobile.core.Classes;
using notification.mobile.core.Services;
using Xamarin.Forms;

namespace notification.mobile.Android.Services
{
    /// <summary>
    /// When the application is started, the Firebase SDK will automatically request a unique token identifier from the Firebase server.
    /// Upon successful request, the OnNewToken method will be called on the FirebaseService class.
    /// </summary>
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class FirebaseService : FirebaseMessagingService
    {
        public override void OnNewToken(string token)
        {
            // NOTE: save token instance locally, or log if desired
            SendRegistrationToServer(token);
        }

        private void SendRegistrationToServer(string token)
        {
            try
            {
                NotificationHub hub = new NotificationHub(AppConstants.NotificationHubName, AppConstants.ListenConnectionString, this);

                // register device with Azure Notification Hub using the token from FCM
                Registration registration = hub.Register(token, AppConstants.SubscriptionTags);

                // subscribe to the SubscriptionTags list with a simple template.
                string pnsHandle = registration.PNSHandle;
                TemplateRegistration templateReg = hub.RegisterTemplate(pnsHandle, "defaultTemplate", AppConstants.FCMTemplateBody, AppConstants.SubscriptionTags);
            }
            catch (Exception e)
            {
                Log.Error(AppConstants.DebugTag, $"Error registering device: {e.Message}");
            }
        }
        
        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);
            string messageBody = string.Empty;

            if (message.GetNotification() != null)
            {
                messageBody = message.GetNotification().Body;
            }
            // NOTE: test messages sent via the Azure portal will be received here
            else
            {
                messageBody = message.Data.Values.First();
            }

            // convert the incoming message to a local notification
            SendLocalNotification(message.Data);
        }
        void SendLocalNotification(IDictionary<string, string> data)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
            // intent.PutExtra("message", data["message"]);

            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

            var notificationBuilder = new NotificationCompat.Builder(this, AppConstants.NotificationChannelName)
                // .SetContentTitle(data["title"])
                .SetSmallIcon(Resource.Drawable.ic_audiotrack_dark)
                .SetContentText(data["message"])
                .SetAutoCancel(true)
                .SetShowWhen(false)
                .SetContentIntent(pendingIntent);


            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                notificationBuilder.SetChannelId(AppConstants.NotificationChannelName);
            }

            var notificationManager = NotificationManager.FromContext(this);
            notificationManager.Notify(0, notificationBuilder.Build());
        }

    }

}