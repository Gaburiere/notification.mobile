using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using notification.mobile.core.Services;
using AndroidX.Core.App;
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

        private bool _channelInitialized = false;
        private int _messageId = -1;
        private NotificationManager _manager;
        
        public event EventHandler LocalNotificationReceived;
        public event EventHandler PushNotificationReceived;

        public int ScheduleNotification(string title, string message)
        {
            if (!this._channelInitialized)
            {
                this.CreateNotificationChannel();
            }

            this._messageId++;

            // notification intent instance
            Intent intent = new Intent(AndroidApp.Context, typeof(MainActivity));
            intent.PutExtra(AppConstants.TitleKey, title);
            intent.PutExtra(AppConstants.MessageKey, message);
            intent.PutExtra(AppConstants.TypeKey, "local");

            // get intent from current activity
            PendingIntent pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, PendingIntentId, intent, PendingIntentFlags.OneShot);
            
            //build notification
            NotificationCompat.Builder builder = new NotificationCompat.Builder(AndroidApp.Context, ChannelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.abc_ic_ab_back_material))
                .SetSmallIcon(Resource.Drawable.ic_vol_unmute)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);

            var notification = builder.Build();
            
            // notify
            this._manager.Notify(this._messageId, notification);

            return this._messageId;
        }

        public void ReceiveLocalNotification(string title, string message)
        {
            var args = new NotificationEventArgs()
            {
                Title = title,
                Message = message,
            };
            this.LocalNotificationReceived?.Invoke(null, args);
        }

        public void ReceivePushNotification(string title, string message)
        {
            var args = new NotificationEventArgs()
            {
                Title = title,
                Message = message,
            };
            this.PushNotificationReceived?.Invoke(this, args);
        }

        private void CreateNotificationChannel()
        {
            // native notification manager instance
            this._manager = (NotificationManager)AndroidApp.Context.GetSystemService(Context.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelNameJava = new Java.Lang.String(ChannelName);
                var channel = new NotificationChannel(ChannelId, channelNameJava, NotificationImportance.Default)
                {
                    Description = ChannelDescription
                };
                
                // channel creation
                this._manager?.CreateNotificationChannel(channel);
            }

            this._channelInitialized = true;
        }
    }
}