using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace xx.UWP.Utils
{
    public class GetIP
    {
        public static string Get()
        {
            var icp = NetworkInformation.GetInternetConnectionProfile();
            if (icp != null
                  && icp.NetworkAdapter != null
                  && icp.NetworkAdapter.NetworkAdapterId != null)
            {
                var name = icp.ProfileName;

                var hostnames = NetworkInformation.GetHostNames();

                foreach (var hn in hostnames)
                {
                    if (hn.IPInformation != null
                        && hn.IPInformation.NetworkAdapter != null
                        && hn.IPInformation.NetworkAdapter.NetworkAdapterId
                                                                   != null
                        && hn.IPInformation.NetworkAdapter.NetworkAdapterId
                                    == icp.NetworkAdapter.NetworkAdapterId)
                    {
                        return hn.CanonicalName.Replace(".", "");
                    }
                }
            }

            return "---";
        }
    }
}
