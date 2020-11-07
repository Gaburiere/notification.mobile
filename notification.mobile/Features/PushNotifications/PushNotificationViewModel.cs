using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using notification.mobile.core.Dtos;

namespace notification.mobile.Features.PushNotifications
{
    public class PushNotificationViewModel: INotifyPropertyChanged
    {
        public ObservableCollection<PushNotificationDto> PushNotifications { get; set; }

        public PushNotificationViewModel()
        {
            this.PushNotifications = new ObservableCollection<PushNotificationDto>();
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}