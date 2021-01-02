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
using xx.Helper.ListDefinitions;
using Newtonsoft.Json;


namespace xx.Utils
{
    public class ReadSettings
    {
        WriteSettings WriteSettings = new WriteSettings();
        public async void Read(MainPage mp)
        {
            try
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    var settingsRead = await DependencyService.Get<IReadWriteSettingsAndroid>().ReadSettingsAsync();
                    if (string.IsNullOrEmpty(settingsRead))
                    {
                        var action = await mp.DisplayAlert("SEADISTAMINE", "WMS SERVERI AADRESS ON SEADISTAMATA. KAS SOOVID SEADISTADA?", "JAH", "EI");
                        if (action)
                        {
                            WriteSettings.Write(mp);
                        }
                    }
                    else
                    {
                        Debug.WriteLine("output ReadSettings " + settingsRead);
                        mp.lstSettings = JsonConvert.DeserializeObject<List<ListOfSettings>>(settingsRead);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ReadSettings " + ex.Message);
            }
        
        }
    }
}
