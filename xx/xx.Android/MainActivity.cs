using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using xx.Droid;
using Android;
using Honeywell.AIDC.CrossPlatform;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms;
using System.Diagnostics;

[assembly: Xamarin.Forms.Dependency(typeof(PlatformDetails))]
[assembly: Xamarin.Forms.Dependency(typeof(xx.Droid.Version_Android))]

namespace xx.Droid
{
    [Activity(Label = "xx", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        
        private const string DEFAULT_READER_KEY = "default";
        private Dictionary<string, BarcodeReader> mBarcodeReaders;
        private bool mContinuousScan = false, mOpenReader = false;
        private BarcodeReader mSelectedReader = null;
        private SynchronizationContext mUIContext = SynchronizationContext.Current;
        private int mTotalContinuousScanCount = 0;
        private bool mSoftContinuousScanStarted = false;
        private bool mSoftOneShotScanStarted = false;
        public IList<BarcodeReaderInfo> readerList = null;
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            string[] perm = new string[] { Manifest.Permission.WriteExternalStorage, Manifest.Permission.WriteExternalStorage };
            RequestPermissions(perm, 325);
            OpenBarcodeReader();

        }
     

        public async void OpenBarcodeReader()
        {
            mBarcodeReaders = new Dictionary<string, BarcodeReader>();
            readerList = await BarcodeReader.GetConnectedBarcodeReaders();
            mSelectedReader = GetBarcodeReader(readerList.First().ScannerName);
            if (!mSelectedReader.IsReaderOpened)
            {
                BarcodeReader.Result result = await mSelectedReader.OpenAsync();
                if (result.Code == BarcodeReader.Result.Codes.SUCCESS || result.Code == BarcodeReader.Result.Codes.READER_ALREADY_OPENED)
                {
                    SetScannerAndSymbologySettings();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("OpenAsync failed, Code:" + result.Code + " Message:" + result.Message);
                    //await DisplayAlert("Error", "OpenAsync failed, Code:" + result.Code + " Message:" + result.Message, "OK");
                }
            }
        }

        public BarcodeReader GetBarcodeReader(string readerName)
        {
            BarcodeReader reader = null;

            if (readerName == DEFAULT_READER_KEY)
            {
                readerName = null;
            }

            if (null == readerName)
            {
                if (mBarcodeReaders.ContainsKey(DEFAULT_READER_KEY))
                {
                    reader = mBarcodeReaders[DEFAULT_READER_KEY];
                }
            }
            else
            {
                if (mBarcodeReaders.ContainsKey(readerName))
                {
                    reader = mBarcodeReaders[readerName];
                }
            }

            if (null == reader)
            {
                reader = new BarcodeReader(readerName);
                reader.BarcodeDataReady += MBarcodeReader_BarcodeDataReady;
                if (null == readerName)
                {
                    mBarcodeReaders.Add(DEFAULT_READER_KEY, reader);
                }
                else
                {
                    mBarcodeReaders.Add(readerName, reader);
                }
            }

            return reader;
        }

        private async void MBarcodeReader_BarcodeDataReady(object sender, BarcodeDataArgs e)
        {
            Console.WriteLine("skänn käis");
            Console.WriteLine(e.Data);
            mUIContext.Post(_ => { UpdateBarcodeInfo(e.Data, e.SymbologyName, e.TimeStamp); }, null);

            if (mContinuousScan)
            {
                mTotalContinuousScanCount++;

            }
            else if (mSoftOneShotScanStarted)
            {
                await mSelectedReader.SoftwareTriggerAsync(false);
                mSoftOneShotScanStarted = false;
            }
        }

        private void UpdateBarcodeInfo(string data, string symbology, DateTime timestamp)
        {
            System.Diagnostics.Debug.WriteLine(data);
            MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "Acknowledged", data);
        }

        public async void CloseBarcodeReader()
        {
            if (mSelectedReader != null && mSelectedReader.IsReaderOpened)
            {
                if (mSoftOneShotScanStarted || mSoftContinuousScanStarted)
                {
                    // Turn off the software trigger.
                    await mSelectedReader.SoftwareTriggerAsync(false);
                    mSoftOneShotScanStarted = false;
                    mSoftContinuousScanStarted = false;
                }

                BarcodeReader.Result result = await mSelectedReader.CloseAsync();
                if (result.Code == BarcodeReader.Result.Codes.SUCCESS)
                {
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("CloseAsync failed, Code:" + result.Code + " Message:" + result.Message);
                    //await DisplayAlert("Error", "CloseAsync failed, Code:" + result.Code + " Message:" + result.Message, "OK");
                }
            }
        }

        private async void SetScannerAndSymbologySettings()
        {
            try
            {
                if (mSelectedReader.IsReaderOpened)
                {
                    Dictionary<string, object> settings = new Dictionary<string, object>()
                    {
                        {mSelectedReader.SettingKeys.TriggerScanMode, mSelectedReader.SettingValues.TriggerScanMode_OneShot },
                        {mSelectedReader.SettingKeys.Code128Enabled, true },
                        {mSelectedReader.SettingKeys.Code39Enabled, true },
                        {mSelectedReader.SettingKeys.Ean8Enabled, true },
                        {mSelectedReader.SettingKeys.Ean8CheckDigitTransmitEnabled, true },
                        {mSelectedReader.SettingKeys.Ean13Enabled, true },
                        {mSelectedReader.SettingKeys.Ean13CheckDigitTransmitEnabled, true },
                        {mSelectedReader.SettingKeys.Interleaved25Enabled, true },
                        {mSelectedReader.SettingKeys.Interleaved25MaximumLength, 100 },
                        {mSelectedReader.SettingKeys.Postal2DMode, mSelectedReader.SettingValues.Postal2DMode_Usps }
                    };

                    BarcodeReader.Result result = await mSelectedReader.SetAsync(settings);
                    if (result.Code != BarcodeReader.Result.Codes.SUCCESS)
                    {
                        System.Diagnostics.Debug.WriteLine("Symbology settings failed, Code:" + result.Code + " Message:" + result.Message);
                        //await DisplayAlert("Error", "Symbology settings failed, Code:" + result.Code +  " Message:" + result.Message, "OK");
                    }
                }
            }
            catch (Exception exp)
            {
                System.Diagnostics.Debug.WriteLine("Symbology settings failed, Message:" + exp.Message);
                //await DisplayAlert("Error", "Symbology settings failed. Message: " + exp.Message, "OK");
            }
        }
    }
    public class PlatformDetails : IMyInterface
    {
        public PlatformDetails()
        {
        }
        public string GetPlatformName()
        {
            return "I am android!";
        }
    }

    public class Version_Android : IAppVersion
    {
        public string GetVersion()
        {
            var context = global::Android.App.Application.Context;

            PackageManager manager = context.PackageManager;
            PackageInfo info = manager.GetPackageInfo(context.PackageName, 0);

            return info.VersionName;
        }

        public int GetBuild()
        {
            var context = global::Android.App.Application.Context;
            PackageManager manager = context.PackageManager;
            PackageInfo info = manager.GetPackageInfo(context.PackageName, 0);

            return info.VersionCode;
        }
    }
}