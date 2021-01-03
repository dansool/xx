using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace xx.Keyboard
{
    public class KeyBoardButtonPress
    {
        private CharacterReceived CharacterReceived = new CharacterReceived();
        public void KeyPress(string classID, MainPage mp)
        {
            string passValue = "";
            switch(classID)
            {
                case "Enter":
                    {
                        passValue = "13";
                        break;
                    }
                case "Close":
                    {
                        passValue = "";
                        mp.grdKeyBoards.IsVisible = false;
                        mp.stkKeyboardKeyboard.IsVisible = false;
                        mp.stkKeyboardNumeric.IsVisible = false;
                        break;
                    }
                case "BackSpace":
                    {
                        passValue = "8";
                        break;
                    }
                case "SwitchToKeyboard":
                    {
                        passValue = "";
                        mp.stkKeyboardKeyboard.IsVisible = true;
                        mp.stkKeyboardNumeric.IsVisible = false;
                        break;
                    }
                case "SwitchToNumpad":
                    {
                        passValue = "";
                        mp.stkKeyboardKeyboard.IsVisible = false;
                        mp.stkKeyboardNumeric.IsVisible = true;
                        break;
                    }
                case "Minus":
                    {
                        passValue = "";
                        break;
                    }
                case "Plus":
                    {
                        passValue = "";
                        break;
                    }
                default:
                    {
                        var ascii_values = classID.Select(x => (int)x);
                        foreach (var r in ascii_values)
                        {
                            passValue = passValue + r;
                        }
                        break;
                    }
            }

            if (!string.IsNullOrEmpty(passValue))
            {
                Debug.WriteLine(classID + " " + "pressed " + " keyCode: " + passValue);
                CharacterReceived.Receive(passValue, mp);
            }
        }
    }
}
