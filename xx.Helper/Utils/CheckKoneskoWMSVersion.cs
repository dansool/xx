using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace xx.Helper.Utils
{
    public class CheckKoneskoWMSVersion
    {
        public async Task<string> Get()
        {
            System.Net.Http.HttpClient httpClient = new HttpClient();
            var httpResponse = await httpClient.GetStringAsync("http://www.develok.ee/xxWMS/Install/xxWMSVersion.txt");
            return httpResponse.ToString();
        }
       
    }
}
