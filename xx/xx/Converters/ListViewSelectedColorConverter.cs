using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.ComponentModel;
using System.Globalization;
using Xamarin.Forms;

namespace xx.Converters
{
    public class ListViewSelectedColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                Color rw = Color.Beige;
                Color redw = Color.Red;
                bool isSelected = (bool)value;
                if (isSelected)
                {
                    return redw;
                }
                else
                {
                    return rw;
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //throw new NotImplementedException();
            return value;
        }
    }
}