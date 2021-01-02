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
using xx.Helper.ListDefinitions;
using Newtonsoft.Json;
using xx.Templates;
using xx.Scanner;
using xx.Helper.WCF;
using xx.Helper.Utils;

namespace xx
{
    public partial class MainPage : ContentPage
    {
        public enum VirtualKeyboardType
        {
            Keyboard,
            Numberic,
            NumbericMinus,
            Combo
        }

        protected override bool OnBackButtonPressed() => true;

        #region Variables
        private App obj = App.Current as App;
        public string ProfileFileName = "HoneywellDecoderSettingsV2.exm";
        YesNo YesNoPage;
        public string scannedValue = null;
        public string currentVersion = "";
        public string currentScannedValue = "";
        public string currentScannedSymbology = "";
        public bool YesNoResult = false;
        public bool YesNoDone = false;
        public string focusedEditor = "";
        public bool wcfDebug = false;

        #endregion

        #region Utils
        public CheckNewVersion CheckNewVersion = new CheckNewVersion();
        public CollapseAllStackPanels CollapseAllStackPanels = new CollapseAllStackPanels();
        public ReadSettings ReadSettings = new ReadSettings();
        public VersionCheck VersionCheck = new VersionCheck();
        public CharacterReceived CharacterReceived = new CharacterReceived();
        public KeyBoardButtonPress KeyBoardButtonPress = new KeyBoardButtonPress();
        public Communicator Communicator = new Communicator();
        public DecompressData DecompressData = new DecompressData();
        #endregion

        #region List
        public List<ListOfSettings> lstSettings = new List<ListOfSettings>();
        public List<ListOfSettings> lstSet = new List<ListOfSettings>();
        #endregion

        #region WCF
        WCFSC_ValidateUser WCFSC_ValidateUser = new WCFSC_ValidateUser();
        #endregion


        #region MainPage operations

        public MainPage()
        {
            InitializeComponent();
            Device.SetFlags(new string[] { "RadioButton_Experimental" }); // to be able to use radio buttons        
            if (Device.RuntimePlatform == Device.UWP) { obj.operatingSystem = "UWP"; UWP(); }
            if (Device.RuntimePlatform == Device.Android) { obj.operatingSystem = "Android"; Android(); }
            //var lst = new List<ListOfSettings>();
            //DisplayAlert("", lst.GetType().ToString(), "OK");
            obj.wcfAddress = "https://wms.konesko.ee/KoneskoWMS";


        }

        public void UWP()
        {
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "exception", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { DisplayAlert("VIGA", arg, "OK"); }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "scannerInitStatus", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { Debug.WriteLine("Scanner initialization is complete " + arg); }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "downloadUpdateProgress", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { lblUpdate.Text = "UUE VERSIOONI LAADIMINE " + arg + "%"; }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "deviceSerial", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { lblDeviceSerial.Text = arg; Debug.WriteLine(arg); obj.deviceSerial = arg;  }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "isDeviceHandheld", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { obj.isDeviceHandheld = Convert.ToBoolean(arg); }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "backPressed", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { txtEdit1.Text = "BackPressed"; }); });

            MessagingCenter.Subscribe<App, string>(this, "KeyboardListener", (sender, args) => { CharacterReceived.Receive(args, this); });

            VersionCheck.Check(this);
            ScannedValueReceive();
            ReadSettings.Read(this);
        }

        public async void Android()
        {
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "exception", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { DisplayAlert("VIGA", arg, "OK"); }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "scannerInitStatus", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { Debug.WriteLine("Scanner initialization is complete"); }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "downloadUpdateProgress", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { lblUpdate.Text = "UUE VERSIOONI LAADIMINE " + arg + "%"; }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "downloadComplete", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { CollapseAllStackPanels.Collapse(this); stkEsimene.IsVisible = true; }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "deviceSerial", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { lblDeviceSerial.Text = arg; Debug.WriteLine(arg); obj.deviceSerial = arg; }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "backPressed", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { txtEdit1.Text = "BackPressed"; }); });

            VersionCheck.Check(this);
            ScannedValueReceive();
            ReadSettings.Read(this);
        }
        #endregion

        #region ScannedValue operations
        public void ScannedValueReceive()
        {
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "scannedValue", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { currentScannedValue = arg; ProcessScannedValue(arg); }); });
        }

        public async void ProcessScannedValue(string scannedValue)
        {
            Debug.WriteLine("ProcessScannedValue " + scannedValue);
            if (scannedValue.Contains("###"))
            {
                var split = scannedValue.Split(new[] { "###" }, StringSplitOptions.None);
                currentScannedValue = split[0];
                currentScannedSymbology = split[1];
            }
            else
            {
                currentScannedValue = scannedValue;
            }
            txtEdit1.Text = currentScannedValue;

            Debug.WriteLine("scanned: " + currentScannedValue + " symbology: " + currentScannedSymbology);
            
            var row = new ListOfSettings { wmsAddress = currentScannedValue };
            lstSet.Add(row);

            Debug.WriteLine(obj.currentCanvasName);

            LstvSettings.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(ViewCell1)) : new DataTemplate(typeof(ViewCell2));
            LstvSettings.ItemsSource = null;
            LstvSettings.ItemsSource = lstSet;

        }
        #endregion

        private void HandleModalPopping(object sender, ModalPoppingEventArgs e)
        {
            if (e.Modal == YesNoPage)
            {
                // now we can retrieve that phone number:
                var result = YesNoPage.YesNoResult;
                YesNoPage = null;
                YesNoResult = result;
                // remember to remove the event handler:
                xx.App.Current.ModalPopping -= HandleModalPopping;
            }
        }

        public async Task<bool> YesNoDialog()
        {
            YesNoPage = new YesNo(this);
            xx.App.Current.ModalPopping += HandleModalPopping;
            await Navigation.PushModalAsync(YesNoPage);
            return YesNoResult;

        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("DeviceSerial = " + obj.deviceSerial);
            Debug.WriteLine("MESSAGE! " + " ENNE OLI " + obj.currentCanvasName);
            obj.currentCanvasName = "Teine";
            stkEsimene.IsVisible = false;
            stkTeine.IsVisible = true;

        }



        private async void Button2_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("MESSAGE! " + " ENNE OLI " + obj.currentCanvasName);
            obj.currentCanvasName = "Esimene";
            stkEsimene.IsVisible = true;
            stkTeine.IsVisible = false;

            List<ListOfUser> lstQuery = new List<ListOfUser> { new ListOfUser { username = "DAN.SOOL", scannerID = obj.deviceSerial, pEnv = 1 } };
            var result = await WCFSC_ValidateUser.Query(lstQuery, obj.wcfAddress, true);
            if (result.Item1)
            {
                txtEdit3.Text = result.Item2.First().username;
            }
            else
            {
                await DisplayAlert("WCF", result.Item3, "OK");
            }
        }


        public void OnEnterPressed(object sender, EventArgs e)
        {
            Debug.WriteLine("txtEdit3 " + txtEdit3.Text);
            txtEdit3.Text = txtEdit3.Text;
        }

        private void txtEdit3_TextChanged(object sender, TextChangedEventArgs e)
        {
            string currentText = e.NewTextValue;
            string lastText = "";
            if (!String.IsNullOrEmpty(e.OldTextValue))
            {
                lastText = e.OldTextValue;
            }
            var currentNumb = currentText.Length - currentText.Replace(Environment.NewLine, string.Empty).Length;
            var lastNumb = lastText.Length - lastText.Replace(Environment.NewLine, string.Empty).Length;
            if (currentNumb > lastNumb)
            {
                txtEdit3.Text = lastText;
                txtEdit3.Unfocus();
            }
        }

        void LstvSettings_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as ListOfSettings;
            lstSet.ForEach(x => x.isSelected = false);
            item.isSelected = true;
        }

        private void TxtEdit3_Focused(object sender, FocusEventArgs e)
        {
            txtEdit3.Focus();
            txtEdit3.Unfocus();
            focusedEditor = "TxtEdit3";
            ShowKeyBoard(VirtualKeyboardType.Combo);
        }

        private void txtEdit4_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TxtEdit4_Focused(object sender, FocusEventArgs e)
        {
            txtEdit4.Focus();
            txtEdit4.Unfocus();
            focusedEditor = "TxtEdit4";
            ShowKeyBoard(VirtualKeyboardType.Combo);
        }

        public void ShowKeyBoard(VirtualKeyboardType type)
        {
            Debug.WriteLine("isDeviceHandheld " + obj.isDeviceHandheld);
            if (obj.isDeviceHandheld)
            {
                stkKeyboardCombo.Margin = new Thickness(0, 400, 0, 0);
                if (obj.operatingSystem == "UWP")
                {
                    stkKeyboardCombo.Margin = new Thickness(0, 300, 0, 0);
                }

                if (type == VirtualKeyboardType.Combo)
                {

                    stkKeyboardCombo.IsVisible = true;
                }
            }
        }

        private void KeyboardButton_Clicked(object sender, EventArgs e)
        {
            var classID = (sender as Button).ClassId;
            KeyBoardButtonPress.KeyPress(classID, this);
        }
    }
  
}
