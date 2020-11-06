using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using notification.mobile.core.Services;
using Android.Support.V4.App;
using AndroidX.Core.App;
using notification.mobile.core;
using AndroidApp = Android.App.Application;

namespace notification.mobile.Android.Services
{
    public class AndroidNotificationManager: INotificationManager
    {
        const string channelId = "default";
        const string channelName = "Default";
        const string channelDescription = "The default channel for notifications.";
        const int pendingIntentId = 0;

        public const string TitleKey = "title";
        public const string MessageKey = "message";

        bool channelInitialized = false;
        int messageId = -1;
        NotificationManager manager;
        
        
        public event EventHandler NotificationReceived;
        public void Initialize()
        {
            this.CreateNotificationChannel();
        }

        public int ScheduleNotification(string title, string message)
        {
            if (!this.channelInitialized)
            {
                this.CreateNotificationChannel();
            }

            this.messageId++;

            Intent intent = new Intent(AndroidApp.Context, typeof(MainActivity));
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);

            PendingIntent pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, pendingIntentId, intent, PendingIntentFlags.OneShot);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(AndroidApp.Context, channelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.ic_mtrl_chip_close_circle))
                .SetSmallIcon(Resource.Drawable.ic_mtrl_chip_close_circle)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);

            var notification = builder.Build();
            this.manager.Notify(this.messageId, notification);

            return this.messageId;
        }

        public void ReceiveNotification(string title, string message)
        {
            var args = new NotificationEventArgs()
            {
                Title = title,
                Message = message,
            };
            this.NotificationReceived?.Invoke(null, args);
        }
        
        private void CreateNotificationChannel()
        {
            this.manager = (NotificationManager)AndroidApp.Context.GetSystemService(Context.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelNameJava = new Java.Lang.String(channelName);
                var channel = new NotificationChannel(channelId, channelNameJava, NotificationImportance.Default)
                {
                    Description = channelDescription
                };
                this.manager?.CreateNotificationChannel(channel);
            }

            this.channelInitialized = true;
        }
    }
}