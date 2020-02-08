using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace xx
{
    public partial class App : Application
    {
       
        public MainPage mp;
        public bool scannerInitComplete;
        public string currentCanvasName = "";
        public bool isDeviceHandheld;
        //public Windows.Devices.PointOfService.ClaimedBarcodeScanner claimedScanner = null;
        //public  Windows.Devices.PointOfService.BarcodeScanner scanner = null;
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
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
