using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xx.Helper.Utils
{
    public class RestAddressSuffix
    {
        public string Get(int productionEnvironment)
        {
            string URI = string.Empty;
            if (productionEnvironment == 0)
            {
                URI = @"/ServiceTest.svc/rest/";
            }
            if (productionEnvironment == 1)
            {
                URI = @"/ServiceProduction.svc/rest/";
            }
            return URI;
        }
    }
}
