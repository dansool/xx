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
using xx.Helper.WCF;
using xx.Helper.Utils;
using xx.Keyboard;

namespace xx
{
    
    public partial class MainPage : ContentPage
    {
        
        
       
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
        public VirtualKeyboardTypes VirtualKeyboardTypes = new VirtualKeyboardTypes();
        public ShowKeyBoard ShowKeyBoard = new ShowKeyBoard();
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
            if (obj.operatingSystem == "UWP")
            {
                grdMain.ScaleX = 1.0;
                grdMain.ScaleY = 1.0;
            }
            if (obj.operatingSystem == "Android")
            {
                grdMain.ScaleX = 1.1;
                grdMain.ScaleY = 1.0;
            }
            obj.wcfAddress = "https://wms.konesko.ee/KoneskoWMS";
           

        }

        public async void UWP()
        {
            grdMain.IsVisible = true;
            stkEsimene.IsVisible = true;

            MessagingCenter.Subscribe<App, string>((App)Application.Current, "exception", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { DisplayAlert("VIGA", arg, "OK"); }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "scannerInitStatus", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { Debug.WriteLine("Scanner initialization is complete " + arg); }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "downloadUpdateProgress", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { lblUpdate.Text = "UUE VERSIOONI LAADIMINE " + arg + "%"; }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "deviceSerial", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { lblDeviceSerial.Text = arg; Debug.WriteLine(arg); obj.deviceSerial = arg;  }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "isDeviceHandheld", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { obj.isDeviceHandheld = Convert.ToBoolean(arg); }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "backPressed", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { entEdit1.Text = "BackPressed"; }); });

            MessagingCenter.Subscribe<App, string>(this, "KeyboardListener", (sender, args) => { CharacterReceived.Receive(args, this); });

            var version = await VersionCheck.Check(this);
            ScannedValueReceive();
            var resultSettings = await ReadSettings.Read(this);
            if (resultSettings.Item1)
            {
             
            }
            if (version.Item1)
            {
                if (lstSettings.Any())
                {
                    lstSettings.First().currentVersion = version.Item3;
                    lblVersion.Text = "Versioon: " + lstSettings.First().currentVersion;
                }
                else
                {
                    lstSettings = new List<ListOfSettings>();
                    lstSettings.Add(new ListOfSettings { currentVersion = version.Item3 });
                    lblVersion.Text = "Versioon: " + lstSettings.First().currentVersion;
                }
            }
            
        }

        public async void Android()
        {
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "exception", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { DisplayAlert("VIGA", arg, "OK"); }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "scannerInitStatus", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { Debug.WriteLine("Scanner initialization is complete"); }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "downloadUpdateProgress", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { lblUpdate.Text = "UUE VERSIOONI LAADIMINE " + arg + "%"; }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "downloadComplete", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { CollapseAllStackPanels.Collapse(this); stkEsimene.IsVisible = true; }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "isDeviceHandheld", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { obj.isDeviceHandheld = Convert.ToBoolean(arg); }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "deviceSerial", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { lblDeviceSerial.Text = arg; Debug.WriteLine(arg); obj.deviceSerial = arg; }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "backPressed", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { entEdit1.Text = "BackPressed"; }); });

            var version = await VersionCheck.Check(this);

            ScannedValueReceive();
            Debug.WriteLine("Siin?");
            var resultSettings = await ReadSettings.Read(this);
            if (resultSettings.Item1)
            {
                
            }
            if (version.Item1)
            {
                if (lstSettings.Any())
                {
                    lstSettings.First().currentVersion = version.Item3;
                    lblVersion.Text = "Versioon: " + lstSettings.First().currentVersion;
                }
            }
            grdMain.IsVisible = true;
            stkEsimene.IsVisible = true;
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
            entEdit1.Text = currentScannedValue;

            Debug.WriteLine("scanned: " + currentScannedValue + " symbology: " + currentScannedSymbology);
            
            var row = new ListOfSettings { wmsAddress = currentScannedValue };
            lstSet.Add(row);

            Debug.WriteLine(obj.currentCanvasName);

            LstvSettings.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(ViewCell1)) : new DataTemplate(typeof(ViewCell2));
            LstvSettings.ItemsSource = null;
            LstvSettings.ItemsSource = lstSet;

        }
        #endregion

       

      
        public async Task<bool> YesNoDialog()
        {
            YesNoPage = new YesNo(this);

            var waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
            var modalPage = new YesNo(this);
            modalPage.Disappearing += (sender2, e2) =>
            {
                waitHandle.Set();
            };
            await this.Navigation.PushModalAsync(modalPage);
            await Task.Run(() => waitHandle.WaitOne());
            return YesNoResult;

        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            //Debug.WriteLine("DeviceSerial = " + obj.deviceSerial);
            //Debug.WriteLine("MESSAGE! " + " ENNE OLI " + obj.currentCanvasName);
            //obj.currentCanvasName = "Teine";
            //stkEsimene.IsVisible = false;
            //stkTeine.IsVisible = true;
            var result = await YesNoDialog();
            if (YesNoResult)
            {
                Debug.WriteLine("vastati jah");
            }
            else
            {
                Debug.WriteLine("vastati ei");
            }
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
                entEdit3.Text = result.Item2.First().username;
            }
            else
            {
                await DisplayAlert("WCF", result.Item3, "OK");
            }
        }

        void LstvSettings_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as ListOfSettings;
            lstSet.ForEach(x => x.isSelected = false);
            item.isSelected = true;
        }

        private void KeyboardButton_Clicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Color btnColor = btn.BackgroundColor;
            btn.BackgroundColor = Color.Gold;
            Device.StartTimer(TimeSpan.FromSeconds(0.05), () => { btn.BackgroundColor = btnColor; return false; });
            var classID = (sender as Button).ClassId;
            KeyBoardButtonPress.KeyPress(classID, this);
        }

        private void Entry_FocusedNumericWithSwitch(object sender, FocusEventArgs e)
        {
            Entry current = sender as Entry;
            current.BackgroundColor = Color.Yellow;
            
            //Needed for Android
            current.Focus();
            current.Unfocus();
            //===

            if (!string.IsNullOrEmpty(focusedEditor))
            {
                if (focusedEditor != current.ClassId)
                {
                    Entry previous = this.FindByName<Entry>(focusedEditor);
                    previous.BackgroundColor = Color.White;
                }
            }
            focusedEditor = current.ClassId;

            ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.NumericWithSwitch, this);
        }

        private void Entry_FocusedNumeric(object sender, FocusEventArgs e)
        {
            Entry current = sender as Entry;
            current.BackgroundColor = Color.Yellow;

            //Needed for Android
            current.Focus();
            current.Unfocus();
            //===

            if (!string.IsNullOrEmpty(focusedEditor))
            {
                if (focusedEditor != current.ClassId)
                {
                    Entry previous = this.FindByName<Entry>(focusedEditor);
                    previous.BackgroundColor = Color.White;
                }
            }
            focusedEditor = current.ClassId;

            ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.Numeric, this);
        }

        private void Entry_FocusedNumericWithPlusMinus(object sender, FocusEventArgs e)
        {
            Entry current = sender as Entry;
            current.BackgroundColor = Color.Yellow;

            //Needed for Android
            current.Focus();
            current.Unfocus();
            //===

            if (!string.IsNullOrEmpty(focusedEditor))
            {
                if (focusedEditor != current.ClassId)
                {
                    Entry previous = this.FindByName<Entry>(focusedEditor);
                    previous.BackgroundColor = Color.White;
                }
            }
            focusedEditor = current.ClassId;

            ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.NumericWithPlusMinus, this);
        }

        private void Entry_FocusedNumericWithSwitchAndPlusMinus(object sender, FocusEventArgs e)
        {
            Entry current = sender as Entry;
            current.BackgroundColor = Color.Yellow;

            //Needed for Android
            current.Focus();
            current.Unfocus();
            //===

            if (!string.IsNullOrEmpty(focusedEditor))
            {
                if (focusedEditor != current.ClassId)
                {
                    Entry previous = this.FindByName<Entry>(focusedEditor);
                    previous.BackgroundColor = Color.White;
                }
            }
            focusedEditor = current.ClassId;

            ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.NumericWithSwitchAndPlusMinus, this);
        }

        private void Entry_FocusedKeyboard(object sender, FocusEventArgs e)
        {
            Entry current = sender as Entry;
            current.BackgroundColor = Color.Yellow;

            //Needed for Android
            current.Focus();
            current.Unfocus();
            //===

            if (!string.IsNullOrEmpty(focusedEditor))
            {
                if (focusedEditor != current.ClassId)
                {
                    Entry previous = this.FindByName<Entry>(focusedEditor);
                    previous.BackgroundColor = Color.White;
                }
            }
            focusedEditor = current.ClassId;

            ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.Keyboard, this);
        }

        private void Entry_FocusedKeyboardWithSwitch(object sender, FocusEventArgs e)
        {
            Entry current = sender as Entry;
            current.BackgroundColor = Color.Yellow;

            //Needed for Android
            current.Focus();
            current.Unfocus();
            //===

            if (!string.IsNullOrEmpty(focusedEditor))
            {
                if (focusedEditor != current.ClassId)
                {
                    Entry previous = this.FindByName<Entry>(focusedEditor);
                    previous.BackgroundColor = Color.White;
                }
            }
            focusedEditor = current.ClassId;

            ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.KeyboardWithSwitch, this);
        }

        private void Entry_FocusedNoEntry(object sender, FocusEventArgs e)
        {
            Entry current = sender as Entry;
            current.BackgroundColor = Color.Yellow;

            //Needed for Android
            current.Focus();
            current.Unfocus();
            //===

            if (!string.IsNullOrEmpty(focusedEditor))
            {
                if (focusedEditor != current.ClassId)
                {
                    Entry previous = this.FindByName<Entry>(focusedEditor);
                    previous.BackgroundColor = Color.White;
                }
            }
            focusedEditor = current.ClassId;
            grdKeyBoards.IsVisible = false;
        }

        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            Entry current = sender as Entry;
            if (focusedEditor != current.ClassId)
            {
                current.BackgroundColor = Color.White;
            }
        }
    }
}
