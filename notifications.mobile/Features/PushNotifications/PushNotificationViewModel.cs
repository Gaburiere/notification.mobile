using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using notification.mobile.core.Classes;
using notification.mobile.core.Dtos;
using notification.mobile.core.Services;
using Xamarin.Forms;

namespace notification.mobile.Features.PushNotifications
{
    public class PushNotificationViewModel: INotifyPropertyChanged
    {
        private INotificationManager _notificationManager;
        public ObservableCollection<PushNotificationDto> PushNotifications { get; set; }

        public PushNotificationViewModel()
        {
            this.PushNotifications = new ObservableCollection<PushNotificationDto>();
            this._notificationManager = DependencyService.Get<INotificationManager>();
            this._notificationManager.PushNotificationReceived += (sender, s) =>
            {
                var notificationEventArgs = (NotificationEventArgs) s;
                this.PushNotifications.Add(new PushNotificationDto()
                {
                    Title = notificationEventArgs.Title,
                    Message = notificationEventArgs.Message
                });
            };
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}