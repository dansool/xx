﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace xx
{
    public partial class App : Application
    {
        public static INavigation GlobalNavigation { get; private set; }

        public MainPage mp;
        public bool scannerInitComplete;
        public string currentCanvasName = "";
        public bool isDeviceHandheld;
        public int pEnv;
        public string deviceSerial;
        public string operatingSystem;
        public string wcfAddress;
        public string currentVersion;
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
