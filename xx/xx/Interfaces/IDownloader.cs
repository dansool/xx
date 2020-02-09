using System;
using System.Collections.Generic;
using System.Text;

namespace xx
{
    public interface IDownloader
    {
        void DownloadFile(string url);
        event EventHandler<DownloadEventArgs> OnFileDownloaded;
    }
}
