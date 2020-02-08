using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace xx.Helper
{
    public class Class1
    {
        public async Task<string> Get()
        {
            System.Net.Http.HttpClient httpClient = new HttpClient();
            var httpResponse = await httpClient.GetStringAsync("http://www.develok.ee/KoneskoWMS/Install/KoneskoWMSVersion.txt");
            return httpResponse.ToString();
        }
       
    }
}
