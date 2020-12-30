using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using System.Diagnostics;

namespace xx.Utils
{
    public class CheckNewVersion
    {
        ParseVersionToList ParseVersionToList = new ParseVersionToList();
        public GetPublishedVersion GetPublishedVersion = new GetPublishedVersion();

        public async Task<Tuple<bool, string, string, string, string>> Check()
        {
            try
            {
                string currentVersion = null;
                string publishedVersion = null;
                var p = 0;
                
               
                var result = await GetPublishedVersion.Get();
                if (result.Item1)
                {
                    publishedVersion = result.Item3;
                    if (Device.RuntimePlatform == Device.UWP)
                    {
                        currentVersion = DependencyService.Get<IPlatformDetailsUWP>().GetPlatformName();
                    }
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        var build = DependencyService.Get<IAppVersion>().GetBuild();
                        var version = DependencyService.Get<IAppVersion>().GetVersion();
                        currentVersion = build + "." + version;
                        p = 3;
                    }
                   
                    if (!string.IsNullOrEmpty(currentVersion))
                    {
                        Debug.WriteLine(currentVersion + "  " + publishedVersion);
                        var lstPublishedVersion = ParseVersionToList.Get(publishedVersion);
                        var lstCurrentVersion = ParseVersionToList.Get(currentVersion);
                        if (currentVersion == publishedVersion || lstCurrentVersion.First().Minor > lstPublishedVersion.First().Minor)
                        {
                            return new Tuple<bool, string, string, string, string>(true, null, publishedVersion, currentVersion, null);
                        }
                        else
                        {
                            
                            if ((lstCurrentVersion.First().Minor < lstPublishedVersion.First().Minor))
                            {
                                
                                return new Tuple<bool, string, string, string, string>(false, "LEITUD UUS VERSIOON " + publishedVersion + "\r\n" + "KAS UUENDADA TARKVARA?", publishedVersion, currentVersion, lstPublishedVersion.First().Minor.ToString());
                            }
                        }
                    }
                    else
                    {
                        return new Tuple<bool, string, string, string, string>(false, "TARKVARA VERSIOONI EI SUUDETUD LEIDA!", null, null, null);
                    }
                   
                    return new Tuple<bool, string, string, string, string>(true, null, publishedVersion, currentVersion, null);
                }
                else
                {
                    return new Tuple<bool, string, string, string, string>(false, "TARKVARA VERSIOONI KONTROLL EBAÕNNESTUS!", null, null, null);
                }
            }

            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Tuple<bool, string, string, string, string>(false, "AAAAA " + ex.Message, null, null, null);
            }
        }

    }
}
