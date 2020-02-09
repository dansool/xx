using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Web.Http;
using System.Threading;
using System.Diagnostics;
using xx.UWP.Utils;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(xx.UWP.Interfaces.PlatformDetailsUWP))]

namespace xx.UWP.Interfaces
{
    public class PlatformDetailsUWP : IPlatformDetailsUWP
    {
        private App obj = App.Current as App;
        

        public string GetPlatformName()
        {
            int Build = Windows.ApplicationModel.Package.Current.Id.Version.Build;
            int Major = Windows.ApplicationModel.Package.Current.Id.Version.Major;
            int Minor = Windows.ApplicationModel.Package.Current.Id.Version.Minor;
            int Revision = Windows.ApplicationModel.Package.Current.Id.Version.Revision;

            return (Major + "." + Minor + "." + Build + "." + Revision).ToString();
        }

        public async Task<string> DownloadAndInstall(string versionFromWeb)
        {
            try
            {
                string fileType = obj.isDeviceHandheld ? "_ARM.appx" : "_x64.appxbundle";
                BackgroundDownloader downloader = new BackgroundDownloader();
                Uri source = new Uri("http://www.develok.ee/KoneskoWMS/Install/KoneskoWMS_" + versionFromWeb + fileType, UriKind.Absolute);
                StorageFolder fold = KnownFolders.PicturesLibrary;
                StorageFile testfile = await fold.CreateFileAsync("KoneskoWMS_" + versionFromWeb + fileType, CreationCollisionOption.ReplaceExisting);

                Progress<DownloadOperation> progress = new Progress<DownloadOperation>(progressChanged);

                DownloadOperation download = downloader.CreateDownload(source, testfile);
                CancellationTokenSource cancellationToken = new CancellationTokenSource();
                await download.StartAsync().AsTask(cancellationToken.Token, progress);
                IReadOnlyList<StorageFile> filesInFolder = await KnownFolders.PicturesLibrary.GetFilesAsync();
                foreach (StorageFile file in filesInFolder)
                {
                    if (file.Name == "KoneskoWMS_" + versionFromWeb + fileType)
                    {
                        if (await Windows.System.Launcher.LaunchFileAsync(file))
                        {
                            return null;
                        }
                        else
                        {
                            return "UUENDAMINE EI ÕNNESTUNUD!";
                        }
                    }
                }
                return "UUENDAMINE EI ÕNNESTUNUD!";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return "UUENDAMINE EI ÕNNESTUNUD! " + "\r\n" + ex.Message;
            }
        }

        private void progressChanged(DownloadOperation downloadOperation)
        {
            try
            {
                int progress = (int)(100 * ((double)downloadOperation.Progress.BytesReceived / (double)downloadOperation.Progress.TotalBytesToReceive));
                MessagingCenter.Send<xx.App, string>((xx.App)obj.xxApp, "downloadUpdateProgress", progress.ToString());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
