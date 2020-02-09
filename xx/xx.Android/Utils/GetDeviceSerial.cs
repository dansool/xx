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
using System.Threading.Tasks;
using System.Diagnostics;
using Android.Provider;
using Xamarin.Forms;

namespace xx.Droid.Utils
{
    public class GetDeviceSerial
    {

        public async Task<Tuple<bool, string, string>> Get()
        {
            try
            {
                string serial = Settings.Secure.GetString(Forms.Context.ContentResolver, Settings.Secure.AndroidId);
                return new Tuple<bool, string, string>(true, null, serial);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string, string>(false, ex.Message, null);
            }
        }
    }
}