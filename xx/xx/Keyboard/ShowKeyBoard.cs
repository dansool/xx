using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Xamarin.Forms;

namespace xx.Keyboard
{
    public class ShowKeyBoard
    {
        private App obj = App.Current as App;

        public void Show(VirtualKeyboardTypes.VirtualKeyboardType type, MainPage mp)
        {
            Debug.WriteLine("isDeviceHandheld " + obj.isDeviceHandheld);
            if (obj.isDeviceHandheld)
            {
                mp.grdKeyBoards.IsVisible = true;
                
                mp.stkKeyboardKeyboard.IsVisible = false;
                mp.stkKeyboardNumeric.IsVisible = false;

                if (obj.operatingSystem == "Android")
                {
                    mp.grdKeyBoards.Margin = new Thickness(6, 360, 0, 0);
                    mp.grdKeyBoards.ScaleX = 1.045;
                    mp.grdKeyBoards.ScaleY = 1.0;
                }
                if (obj.operatingSystem == "UWP")
                {
                    mp.grdKeyBoards.Margin = new Thickness(0, 270, 0, 0);
                    mp.grdKeyBoards.ScaleX = 1.03;
                    mp.grdKeyBoards.ScaleY = 1.0;
                }

                if (type == VirtualKeyboardTypes.VirtualKeyboardType.Keyboard)
                {
                    mp.stkKeyboardKeyboard.IsVisible = true;
                }
                if (type == VirtualKeyboardTypes.VirtualKeyboardType.KeyboardWithSwitch)
                {
                    mp.stkKeyboardKeyboard.IsVisible = true;
                    mp.stkKeyboardNumericSwitchToKeyboard.IsVisible = true;
                    mp.stkKeyboardNumericSwitchToNumpad.IsVisible = true;
                    mp.stkMinus.IsVisible = false;
                    mp.stkPlus.IsVisible = false;
                }
                if (type == VirtualKeyboardTypes.VirtualKeyboardType.NumericWithSwitch)
                {
                    mp.stkKeyboardNumeric.IsVisible = true;
                    mp.stkKeyboardNumericSwitchToKeyboard.IsVisible = true;
                    mp.stkKeyboardNumericSwitchToNumpad.IsVisible = true;
                    mp.stkMinus.IsVisible = false;
                    mp.stkPlus.IsVisible = false;
                }
                if (type == VirtualKeyboardTypes.VirtualKeyboardType.Numeric)
                {
                    mp.stkKeyboardNumeric.IsVisible = true;
                    mp.stkKeyboardNumericSwitchToKeyboard.IsVisible = false;
                    mp.stkKeyboardNumericSwitchToNumpad.IsVisible = false;
                    mp.stkMinus.IsVisible = false;
                    mp.stkPlus.IsVisible = false;
                }
                if (type == VirtualKeyboardTypes.VirtualKeyboardType.NumericWithSwitchAndPlusMinus)
                {
                    mp.stkKeyboardNumeric.IsVisible = true;
                    mp.stkKeyboardNumericSwitchToKeyboard.IsVisible = true;
                    mp.stkKeyboardNumericSwitchToNumpad.IsVisible = true;
                    mp.stkMinus.IsVisible = true;
                    mp.stkPlus.IsVisible = true;
                }

               
            }

        }
    }
}
