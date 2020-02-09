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

namespace xx.Utils
{
    public class VersionCheck
    {
        MainPage mps;
        public async void Check(MainPage mp)
        {
            mps = mp;
            IDownloader downloader = DependencyService.Get<IDownloader>();

            var versionCheck = await mp.CheckNewVersion.Check();
            if (!versionCheck.Item1)
            {
                mp.lblVersion.Text = "Versioon: " + versionCheck.Item4;
                if (versionCheck.Item2.StartsWith("LEITUD UUS VERSIOON"))
                {
                    var action = await mp.DisplayAlert("UUENDUS", "KAS INSTALLIDA UUS VERSIOON", "JAH", "EI");
                    if (action)
                    {

                        mp.CollapseAllStackPanels.Collapse(mp);
                        mp.stkUpdate.IsVisible = true;
                        if (Device.RuntimePlatform == Device.UWP)
                        {
                            mp.currentVersion = await DependencyService.Get<IPlatformDetailsUWP>().DownloadAndInstall(versionCheck.Item3);
                        }
                        if (Device.RuntimePlatform == Device.Android)
                        {
                            downloader.OnFileDownloaded += OnFileDownloaded;
                            downloader.DownloadFile("http://www.develok.ee/KoneskoWMS/Install/test3.test" + versionCheck.Item5 + ".apk");

                        }
                    }
                    else
                    {
                        mp.CollapseAllStackPanels.Collapse(mp);
                        mp.stkEsimene.IsVisible = true;
                    }
                }
            }
        }

        private void OnFileDownloaded(object sender, DownloadEventArgs e)
        {
            if (!e.FileSaved)
            {
                mps.DisplayAlert("UUENDAMINE", "INSTALLIFAILI EI LEITUD!", "OK");
            }
        }
    }
}
