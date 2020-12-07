using System;

namespace notification.mobile.core.Services
{
    public interface INotificationManager
    {
        event EventHandler LocalNotificationReceived;
        event EventHandler PushNotificationReceived;
        void Initialize();
        int ScheduleNotification(string title, string message);
        void ReceiveLocalNotification(string title, string message);
        void ReceivePushNotification(string title, string message);
    }
}