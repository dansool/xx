using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using xx.Droid.Utils;

[assembly: Dependency(typeof(ReadWriteSettings))]
namespace xx.Droid.Utils
{
    public class ReadWriteSettings : IReadWriteSettingsAndroid
    {
        public async Task<string> SaveSettingsAsync(string settings)
        {
            try
            {
                var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "KoneskoWMSSettings2.txt");
                using (var writer = File.CreateText(backingFile))
                {
                    await writer.WriteLineAsync(settings);
                    return null;
                }
            }
            catch (Exception ex)
            {
                return "SaveSettingsAsync " + ex.Message;
            }
        }

        public async Task<string> ReadSettingsAsync()
        {
            var result = "";
            var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "KoneskoWMSSettings2.txt");

            if (backingFile == null || !File.Exists(backingFile))
            {
                return "";
            }

           
            using (var reader = new StreamReader(backingFile, true))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    result = line;
                }
            }

            return result;
        }
    }
}