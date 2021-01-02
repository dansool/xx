using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace xx.Scanner
{
    public class CharacterReceived
    {
        public async void Receive(string keyCode, MainPage mp)
        {
            if (keyCode == "13")
            {
                Debug.WriteLine("ENTER PRESSED!");
            }
            else
            {
                Debug.WriteLine(mp.focusedEditor + "  " + Convert.ToChar(Convert.ToInt32(keyCode)));
            }
        }
    }
}
