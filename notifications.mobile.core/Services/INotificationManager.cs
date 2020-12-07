using System;

namespace notification.mobile.core.Services
{
    public interface INotificationManager
    {
        event EventHandler<string> NotificationReceived;
        void Initialize();
        int ScheduleNotification(string title, string message);
        void ReceiveNotification(string title, string message);
    }
}