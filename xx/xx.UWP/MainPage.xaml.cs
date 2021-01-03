using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Windows.Devices.Enumeration;
using Windows.Storage.Streams;
using Windows.Devices.PointOfService;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml;
using Xamarin.Forms;
using xx.UWP.Utils;
using Windows.System.Profile;
using Windows.Networking.Connectivity;
using xx.UWP.Scanner;
using Windows.UI.Core;

[assembly: Xamarin.Forms.Dependency(typeof(xx.UWP.Interfaces.PlatformDetailsUWP))]

namespace xx.UWP
{
    public sealed partial class MainPage
    {
        #region variables
        
        //private xx.App xxApp;
        private App obj = App.Current as App;
        ScannerInit ScannerInit = new ScannerInit();
        #endregion

        #region Lists
        #endregion

        #region Utilis
        GetDeviceSerial GetDeviceSerial = new GetDeviceSerial();
        
        #endregion

        public MainPage()
        {
            this.InitializeComponent();
            Windows.UI.Xaml.Application.Current.Resuming += OnAppResuming;
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            obj.xxApp = new xx.App();
            LoadApplication(obj.xxApp);
            LaunchStart();
            Windows.UI.Core.CoreWindow.GetForCurrentThread().KeyDown += HandleKeyDown;
            Window.Current.CoreWindow.CharacterReceived += CoreWindow_CharacterReceived;

            
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
           
            MessagingCenter.Send<xx.App, string>((xx.App)obj.xxApp, "backPressed", "");
        }
        private async void OnAppResuming(object sender, object e)
        {
            if (AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.Desktop")
            {
                await StartScanner();
            }
        }

        protected async override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            if (AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.Desktop")
            {
                await StartScanner();
            }
            base.OnNavigatedTo(e);
        }

        protected async override void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            await ReleaseScanner();
            base.OnNavigatedFrom(e);
        }

        public void HandleKeyDown(Windows.UI.Core.CoreWindow window, Windows.UI.Core.KeyEventArgs e)
        {
           // MessagingCenter.Send<xx.App, string>((xx.App)obj.xxApp, "KeyboardListener", e.VirtualKey.ToString());
        }

        private void CoreWindow_CharacterReceived(CoreWindow sender, CharacterReceivedEventArgs args)
        {
            MessagingCenter.Send<xx.App, string>((xx.App)obj.xxApp, "KeyboardListener", args.KeyCode.ToString());
        }

        public async void UpdateMessageDisplayUpdateOK()
        {
            
        }

        public void UpdateMessageDisplayUpdateNOK()
        {
           
        }


        public async void LaunchStart()
        {
            #region deviceSerial
            if (AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.Desktop")
            {
                var result = await GetDeviceSerial.Get();
                if (result.Item1)
                {
                    if (result.Item3.StartsWith("75EX"))
                    {
                        obj.isDeviceHandheld = true;
                        obj.isHoneyWell = true;
                        obj.deviceSerial = result.Item3.Replace("75EX", "");
                        MessagingCenter.Send<xx.App, string>((xx.App)obj.xxApp, "isDeviceHandheld", "true");
                        MessagingCenter.Send<xx.App, string>((xx.App)obj.xxApp, "deviceSerial", obj.deviceSerial);
                    }
                }
                else
                {
                    MessagingCenter.Send<xx.App, string>((xx.App)obj.xxApp, "deviceSerial", "ERROR " + result.Item2);
                }
            }
            else
            {
                var hostNames = NetworkInformation.GetHostNames();
                obj.deviceSerial = hostNames.First().DisplayName.Replace(".konesko.ee", "");
                MessagingCenter.Send<xx.App, string>((xx.App)obj.xxApp, "isDeviceHandheld", "false");
                MessagingCenter.Send<xx.App, string>((xx.App)obj.xxApp, "deviceSerial", obj.deviceSerial);
            }
            #endregion
        }

        public async Task<bool> StartScanner()
        {
            var x = await ScannerInit.ReloadScannerAsync();
            if (x.Item1)
            {
                App.claimedScanner.DataReceived -= ScannerInit.ClaimedScanner_DataReceivedAsync;
                App.claimedScanner.DataReceived += ScannerInit.ClaimedScanner_DataReceivedAsync;
                return true;
            }
            return false;
        }

        public async Task<bool> ReleaseScanner()
        {
            ScannerInit ScannerInit = new ScannerInit();
            var x = await ScannerInit.ReleaseScannerAsync(true);
            return true;
        }
    }
}
