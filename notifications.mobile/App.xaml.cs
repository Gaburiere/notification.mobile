using notification.mobile.core.Classes;
using notification.mobile.Features;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace notification.mobile
{
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            var mainShell = new MainShell();
            this.MainPage = mainShell;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}