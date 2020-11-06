using System;
using notification.mobile.core;
using notification.mobile.core.Services;
using UserNotifications;

namespace notification.mobile.iOS.Services
{
    public class iOSNotificationManager: INotificationManager
    {
        int messageId = -1;
        bool hasNotificationsPermission;
        
        public event EventHandler NotificationReceived;
        
        public void Initialize()
        {
            // request the permission to use local notifications
            UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert, (approved, err) =>
            {
                if(err != null)
                    throw new Exception($"Failed to request authorization: {err}");
                this.hasNotificationsPermission = approved;
            });
        }

        public int ScheduleNotification(string title, string message)
        {
            // EARLY OUT: app doesn't have permissions
            if(!this.hasNotificationsPermission)
            {
                return -1;
            }

            this.messageId++;

            var content = new UNMutableNotificationContent()
            {
                Title = title,
                Subtitle = "",
                Body = message,
                Badge = 1
            };

            // Local notifications can be time or location based
            // Create a time-based trigger, interval is in seconds and must be greater than 0
            var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(0.25, false);

            var request = UNNotificationRequest.FromIdentifier(this.messageId.ToString(), content, trigger);
            UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) =>
            {
                if (err != null)
                    throw new Exception($"Failed to schedule notification: {err}");
            });

            return this.messageId;
        }

        public void ReceiveNotification(string title, string message)
        {
            var args = new NotificationEventArgs()
            {
                Title = title,
                Message = message
            };
            this.NotificationReceived?.Invoke(null, args);
        }
    }
}