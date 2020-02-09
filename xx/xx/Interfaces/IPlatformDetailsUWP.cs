using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace xx
{
    public interface IPlatformDetailsUWP
    {
        string GetPlatformName();

        Task<string> DownloadAndInstall(string version);
    }
}
