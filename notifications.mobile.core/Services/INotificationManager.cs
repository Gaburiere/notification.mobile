using System;
namespace notification.mobile.core.Services
{
    public interface INotificationManager
    {
        event EventHandler LocalNotificationReceived;
        int ScheduleNotification(string title, string message);
        void ReceiveLocalNotification(string title, string message);
        
        event EventHandler PushNotificationReceived;
        void ReceivePushNotification(string title, string message);


    }
}