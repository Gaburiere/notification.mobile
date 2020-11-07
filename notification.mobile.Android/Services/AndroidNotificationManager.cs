using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using notification.mobile.core.Services;
using AndroidX.Core.App;
using notification.mobile.core;
using notification.mobile.core.Classes;
using AndroidApp = Android.App.Application;

namespace notification.mobile.Android.Services
{
    public class AndroidNotificationManager: INotificationManager
    {
        private const string ChannelId = "default";
        private const string ChannelName = "Default";
        private const string ChannelDescription = "The default channel for notifications.";
        private const int PendingIntentId = 0;

        public const string TitleKey = "title";
        public const string MessageKey = "message";

        private bool _channelInitialized = false;
        private int _messageId = -1;
        private NotificationManager _manager;
        
        
        public event EventHandler NotificationReceived;
        public void Initialize()
        {
            this.CreateNotificationChannel();
        }

        public int ScheduleNotification(string title, string message)
        {
            if (!this._channelInitialized)
            {
                this.CreateNotificationChannel();
            }

            this._messageId++;

            Intent intent = new Intent(AndroidApp.Context, typeof(MainActivity));
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);

            PendingIntent pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, PendingIntentId, intent, PendingIntentFlags.OneShot);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(AndroidApp.Context, ChannelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.ic_mtrl_chip_close_circle))
                .SetSmallIcon(Resource.Drawable.ic_mtrl_chip_close_circle)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);

            var notification = builder.Build();
            this._manager.Notify(this._messageId, notification);

            return this._messageId;
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
            this._manager = (NotificationManager)AndroidApp.Context.GetSystemService(Context.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelNameJava = new Java.Lang.String(ChannelName);
                var channel = new NotificationChannel(ChannelId, channelNameJava, NotificationImportance.Default)
                {
                    Description = ChannelDescription
                };
                this._manager?.CreateNotificationChannel(channel);
            }

            this._channelInitialized = true;
        }
    }
}