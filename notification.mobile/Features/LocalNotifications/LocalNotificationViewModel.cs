using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using notification.mobile.core;
using notification.mobile.core.Classes;
using notification.mobile.core.Services;
using Xamarin.Forms;

namespace notification.mobile.Features.LocalNotifications
{
    public class LocalNotificationViewModel: INotifyPropertyChanged
    {
        private readonly INotificationManager _notificationManager;
        private string _message;
        private string _title;
        public ICommand LocalNotifyCommand { get; private set; }

        public LocalNotificationViewModel()
        {
            this._notificationManager = DependencyService.Get<INotificationManager>();
            this._notificationManager.Initialize();
            this._notificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArgs)eventArgs;
                this.ShowNotification(evtData.Title, evtData.Message);
            };
            
            this.LocalNotifyCommand = new Command(() => this.InnerLocalNotify());
        }

        private void ShowNotification(string title, string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var msg = new Label()
                {
                    Text = $"Notification Received:\nTitle: {title}\nMessage: {message}"
                };
                
            });
        }

        private void InnerLocalNotify()
        {
            this._notificationManager.ScheduleNotification(this.Title, this.Message);
        }

        public string Message
        {
            get => this._message;
            set
            {
                if (this._message == value) return;
                this._message = value;
                this.OnPropertyChanged();
            } 
        }

        public string Title
        {
            get => this._title;
            set
            {
                if (this._title == value) return;
                this._title = value;
                this.OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}