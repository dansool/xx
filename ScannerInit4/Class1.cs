using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.PointOfService;
using System.Net.Http;

namespace xx.ScannerInit4
{
    public class Class1
    {
        public string xxyy()
        {
            return "toimis";
        }

        public async Task<string> Version()
        {
            System.Net.Http.HttpClient httpClient = new HttpClient();
            var httpResponse = await httpClient.GetStringAsync("http://www.develok.ee/KoneskoWMS/Install/KoneskoWMSVersion.txt");
            return httpResponse.ToString();
        }
    }
}
