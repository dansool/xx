using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms;
using Honeywell.AIDC.CrossPlatform;
using xx.Utils;
using xx.StackPanelOperations;
using xx.Helper.ListOfDefinitions;
using Newtonsoft.Json;

namespace xx.Utils
{
    public class WriteSettings
    {
        private App obj = App.Current as App;
        public async void Write(MainPage mp)
        {
            try
            {
                mp.lstSettings = new List<ListOfSettings>();
                var row = new ListOfSettings
                {
                    pEnv = obj.pEnv,
                    wmsAddress = "http://wms.konesko.ee/KoneskoWMS"
                };
                mp.lstSettings.Add(row);
                var settings = new JsonSerializerSettings() { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat };
                string input = JsonConvert.SerializeObject(mp.lstSettings, settings);

                var settingsWrite = await DependencyService.Get<IReadWriteSettingsAndroid>().SaveSettingsAsync(input);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("WriteSettings " + ex.Message);
            }

        }
    }
}
