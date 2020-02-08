using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms;
using Honeywell.AIDC.CrossPlatform;

namespace xx
{
    public interface IMyInterface
    {
        string GetPlatformName();
    }

    public interface IDownloader
    {
        void DownloadFile(string url, string folder);
        event EventHandler<DownloadEventArgs> OnFileDownloaded;
    }
   
    public interface IAppVersion
    {
        string GetVersion();
        int GetBuild();
    }

    public partial class MainPage : ContentPage
    {


        private App obj = App.Current as App;
        private const string DEFAULT_READER_KEY = "default";
        private Dictionary<string, BarcodeReader> mBarcodeReaders;
        private bool mContinuousScan = false, mOpenReader = false;
        private BarcodeReader mSelectedReader = null;
        private SynchronizationContext mUIContext = SynchronizationContext.Current;
        private int mTotalContinuousScanCount = 0;
        private bool mSoftContinuousScanStarted = false;
        private bool mSoftOneShotScanStarted = false;
        public IList<BarcodeReaderInfo> readerList = null;
        public string scannedValue = null;
        public string ProfileFileName = "HoneywellDecoderSettingsV2.exm";
        IDownloader downloader = DependencyService.Get<IDownloader>();

        public string publishedVersion = "";
        public string currentVersion = "";

        protected override void OnAppearing()
        {
            base.OnAppearing();
            

        }

        public MainPage()
        {
            InitializeComponent();
            
            if (Device.RuntimePlatform == Device.UWP)
            {
                UWP();
                MessagingCenter.Subscribe<App, string>((App)Application.Current, "Acknowledged", (sender, arg) =>
                {
                    Debug.WriteLine("GOT IT!");
                    Debug.WriteLine("GOT IT! " + arg);
                    scannedValue = arg.ToString();
                    mScanDataEditor.Text = arg.ToString();
                    mScanDataEditor2.Text = arg.ToString();
                });
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                Android();
                MessagingCenter.Subscribe<App, string>((App)Application.Current, "Acknowledged", (sender, arg) =>
                {
                    DisplayAlert("aaa", "Full Name : " + arg, "OK");
                });
            }


        }

        public async void DisplayMessage(string message)
        {
            await DisplayAlert("aaa", "Full Name : " + message, "OK");
        }

        public void ScanMessage(string message)
        {
            DisplayAlert("aaa", "Full Name : " + message, "OK");
        }
       

        private void OnFileDownloaded(object sender, DownloadEventArgs e)
        {
            if (!e.FileSaved)
            {
                DisplayAlert("UUENDAMINE", "INSTALLIFAILI EI LEITUD!", "OK");
            }
        }

       

        public async void UWP()
        {

            var s = new xx.Helper.Class1();
            publishedVersion = await s.Get();
            var res = DependencyService.Get<IMyInterface>();
            currentVersion = res.GetPlatformName();
            Debug.WriteLine("PublishedVersion : " + publishedVersion);
            Debug.WriteLine("CurrentVersion : " + currentVersion);

            
            //var x = await ReloadScannerAsync(this);
            //if (x.Item1)
            //{
            //    obj.claimedScanner.DataReceived -= ClaimedScanner_DataReceivedAsync;
            //    obj.claimedScanner.DataReceived += ClaimedScanner_DataReceivedAsync;
            //    //await DisplayAlert("Error", x.Item2, "OK");
            //}
        }

        public async void Android()
        {
            var s = new xx.Helper.Class1();
            publishedVersion = await s.Get();
            string v = DependencyService.Get<IAppVersion>().GetVersion();
            int b = DependencyService.Get<IAppVersion>().GetBuild();
            currentVersion = b + "." + v;
            Debug.WriteLine("PublishedVersion : " + publishedVersion);
            Debug.WriteLine("CurrentVersion : " + currentVersion);
            if (publishedVersion != currentVersion)
            {
                var action = await DisplayAlert("UUENDUS", "KAS INSTALLIDA UUS VERSIOON", "Jah", "Ei");
                if (action)
                {
                    downloader.OnFileDownloaded += OnFileDownloaded;
                    downloader.DownloadFile("http://www.develok.ee/KoneskoWMS/Install/test3.test8.apk", "XF_Downloads");
                }
            }
            //OpenBarcodeReader();
        }

       

        private void Button_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("MESSAGE! " + " ENNE OLI " + obj.currentCanvasName);
            obj.currentCanvasName = "Teine";
            Esimene.IsVisible = false;
            Teine.IsVisible = true;
        }

        private void Button2_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("MESSAGE! " + " ENNE OLI " + obj.currentCanvasName);
            obj.currentCanvasName = "Esimene";
            Esimene.IsVisible = true;
            Teine.IsVisible = false;
        }
    }


    //private void DownloadClicked(object sender, EventArgs e)
    //{
    //    if (Device.RuntimePlatform == Device.UWP)
    //    {
    //        if (Device.Idiom == TargetIdiom.Desktop)
    //        {
    //            Debug.WriteLine("DESKTOP!");
    //        }
    //    }
    //    else if (Device.RuntimePlatform == Device.Android)
    //    {
    //        downloader.DownloadFile("http://www.develok.ee/KoneskoWMS/Install/test3.test8.apk", "XF_Downloads");
    //    }
    //}



   
}
