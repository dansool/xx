using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms;
using Honeywell.AIDC.CrossPlatform;
using xx.Utils;
using xx.StackPanelOperations;
using xx.Helper.ListOfDefinitions;
using Newtonsoft.Json;

namespace xx
{
    public partial class MainPage : ContentPage
    {
        #region Variables
        private App obj = App.Current as App;
        public string scannedValue = null;
        public string currentVersion = "";
        #endregion

        #region Utils
        public CheckNewVersion CheckNewVersion = new CheckNewVersion();
        public CollapseAllStackPanels CollapseAllStackPanels = new CollapseAllStackPanels();
        public ReadSettings ReadSettings = new ReadSettings();
        public VersionCheck VersionCheck = new VersionCheck();
        #endregion

        #region List
        public List<ListOfSettings> lstSettings = new List<ListOfSettings>();
        #endregion

        #region WCF
        #endregion


        #region MainPage operations
        public MainPage()
        {
            InitializeComponent();
           
            if (Device.RuntimePlatform == Device.UWP) { UWP(); }
            if (Device.RuntimePlatform == Device.Android) { Android(); }
        }
       
        public void UWP()
        {
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "exception", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { DisplayAlert("VIGA", arg, "OK"); }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "scannerInitStatus", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { Debug.WriteLine("Scanner initialization is complete " + arg); }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "downloadUpdateProgress", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { lblUpdate.Text = "UUE VERSIOONI LAADIMINE " + arg + "%"; }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "deviceSerial", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { lblDeviceSerial.Text = arg; Debug.WriteLine(arg); txtEdit1.Text = arg; txtEdit2.Text = arg; }); });

            VersionCheck.Check(this);
            ReadSettings.Read(this);
            ScannedValueReceive();
        }

        public async void Android()
        {
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "exception", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { DisplayAlert("VIGA", arg, "OK"); }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "scannerInitStatus", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { Debug.WriteLine("Scanner initialization is complete"); }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "downloadUpdateProgress", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { lblUpdate.Text = "UUE VERSIOONI LAADIMINE " + arg + "%"; }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "downloadComplete", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { CollapseAllStackPanels.Collapse(this); stkEsimene.IsVisible = true; }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "deviceSerial", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { lblDeviceSerial.Text = arg; Debug.WriteLine(arg); txtEdit1.Text = arg; txtEdit2.Text = arg; }); });

            VersionCheck.Check(this);
            ScannedValueReceive();
            ReadSettings.Read(this);
        }
        #endregion

        #region ScannedValue operations
        public void ScannedValueReceive()
        {
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "scannedValue", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { ProcessScannedValue(arg); txtEdit1.Text = arg; txtEdit2.Text = arg; }); });
        }

        public void ProcessScannedValue(string scannedValue)
        {
            Debug.WriteLine("ProcessScannedValue " + scannedValue);
        }
        #endregion

        private void Button_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("MESSAGE! " + " ENNE OLI " + obj.currentCanvasName);
            obj.currentCanvasName = "Teine";
            stkEsimene.IsVisible = false;
            stkTeine.IsVisible = true;
        }  

        private void Button2_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("MESSAGE! " + " ENNE OLI " + obj.currentCanvasName);
            obj.currentCanvasName = "Esimene";
            stkEsimene.IsVisible = true;
            stkTeine.IsVisible = false;
        }
    }
}
