using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using xx.Droid;
using System.IO;
using System.Net;
using System.ComponentModel;
using Android.Webkit;
using Java.IO;
using xx.Droid.Utils;
using System.Diagnostics;

[assembly: Dependency(typeof(AndroidDownloader))]
namespace xx.Droid.Utils
{
    public class AndroidDownloader : IDownloader
    {
        public event EventHandler<DownloadEventArgs> OnFileDownloaded;
        string pathToNewFolder = "";
        string fileName = "";
        string urlGet = "";


        public void DownloadFile(string url)
        {
            pathToNewFolder = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "Pictures", "Screenshots");
            try
            {
                urlGet = url;
                using (WebClient webClient = new WebClient())
                {
                    webClient.OpenRead(url);
                    double totalBytes = Convert.ToDouble(webClient.ResponseHeaders["Content-Length"]);

                    webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                    webClient.DownloadProgressChanged += (o, e) =>
                    {
                        double bytesIn = e.BytesReceived;
                        double percentage = ((bytesIn / totalBytes) * 100);
                        MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "downloadUpdateProgress", Math.Truncate(percentage).ToString());
                    };
                   
                    fileName = Path.GetFileName(url);
                    string pathToNewFile = Path.Combine(pathToNewFolder, Path.GetFileName(url));
                    webClient.DownloadFileAsync(new Uri(url), pathToNewFile);
                }
            }
            catch (Exception ex)
            {
                if (OnFileDownloaded != null)
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(false));
            }
        }
        

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    if (OnFileDownloaded != null)
                        OnFileDownloaded.Invoke(this, new DownloadEventArgs(false));
                }
                else
                {
                    MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "downloadComplete", "true");
                    StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
                    StrictMode.SetVmPolicy(builder.Build());

                    Java.IO.File file = new Java.IO.File(pathToNewFolder, Path.GetFileName(urlGet));
                    string extension = MimeTypeMap.GetFileExtensionFromUrl(Android.Net.Uri.FromFile(file).ToString());
                    string mimeType = MimeTypeMap.Singleton.GetMimeTypeFromExtension(extension);
                    Intent intent = new Intent(Intent.ActionView);
                    intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                    intent.SetDataAndType(Android.Net.Uri.FromFile(file), "application/vnd.android.package-archive");

                    intent.AddFlags(ActivityFlags.GrantReadUriPermission);
                    Forms.Context.StartActivity(Intent.CreateChooser(intent, "Your title"));

                    if (OnFileDownloaded != null)
                        OnFileDownloaded.Invoke(this, new DownloadEventArgs(true));
                }
            }
            catch(Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }
    }
}