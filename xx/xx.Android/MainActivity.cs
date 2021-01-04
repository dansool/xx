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
using xx.Droid.Scanner;
using xx.Droid.Utils;
using Android.Content;
using Android.Views;
using Android.Views.InputMethods;

[assembly: Xamarin.Forms.Dependency(typeof(xx.Droid.Utils.PlatformDetailsAndroid))]
[assembly: Xamarin.Forms.Dependency(typeof(xx.Droid.Utils.Version_Android))]


namespace xx.Droid
{
    [Activity(
        Label = "xx", 
        Icon = "@mipmap/icon", 
        Theme = "@style/MainTheme", 
        MainLauncher = false, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        #region Lists
        #endregion

        #region Utilis
        GetDeviceSerial GetDeviceSerial = new GetDeviceSerial();
        public ScannerInit ScannerInit = new ScannerInit();

        #endregion



        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            string[] perm = new string[] { Manifest.Permission.WriteExternalStorage, Manifest.Permission.WriteExternalStorage };
            RequestPermissions(perm, 325);
            ScannerInit.OpenBarcodeReader();
            LaunchStart();

        }

      

        public async void LaunchStart()
        {
            var result = await GetDeviceSerial.Get();
            if (result.Item1)
            {
                string deviceSerial = result.Item3;
                MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "deviceSerial", deviceSerial);
                MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "isDeviceHandheld", "true");
            }
            else
            {
                MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "deviceSerial", "ERROR " + result.Item2);
            }
        }

        public override void OnBackPressed()
        {
            MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "backPressed", "");
            base.OnBackPressed();
        }
    }
    
}