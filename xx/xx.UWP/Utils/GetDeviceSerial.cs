using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Honeywell;
using System.Diagnostics;
using Windows.System.Profile;
using Windows.Networking.Connectivity;
using xx.UWP.Interfaces;
using Xamarin.Forms;

namespace xx.UWP.Utils
{
    public class GetDeviceSerial
    {
        xx.App xxApp;
        private App obj = App.Current as App;
        private GetIP GetIP = new GetIP();
        public async Task<Tuple<bool, string, string>> Get()
        {
            try
            {
                string serial = null;
                if (AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.Desktop")
                {
                    serial = Commands.SerialNumber;
                    return new Tuple<bool, string, string>(true, null, serial);
                }
                else
                {
                    var hostNames = NetworkInformation.GetHostNames();
                    serial = hostNames.First().DisplayName.Replace(".konesko.ee", "");
                    return new Tuple<bool, string, string>(true, null, serial);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new Tuple<bool, string, string>(false, ex.Message, null);
            }


        }
    }
}
