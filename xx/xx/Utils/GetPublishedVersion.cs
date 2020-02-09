using System;
using System.Collections.Generic;
using System.Text;
using xx.Helper.ListDefinitions;
using xx.Helper.Utils;
using System.Threading.Tasks;
using System.Diagnostics;
using xx.Utils;

namespace xx
{
    public class GetPublishedVersion
    {
        CheckKoneskoWMSVersion CheckKoneskoWMSVersion = new CheckKoneskoWMSVersion();
        public async Task<Tuple<bool, string, string>> Get()
        {
            try
            {
                var result = await CheckKoneskoWMSVersion.Get();
                return new Tuple<bool, string, string>(true, null, result);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string, string>(false, "GetPublishedVersion " + ex.Message, null);
            }
        }
    }
}
