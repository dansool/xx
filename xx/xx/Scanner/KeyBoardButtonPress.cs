using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace xx.Scanner
{
    public class KeyBoardButtonPress
    {
        private CharacterReceived CharacterReceived = new CharacterReceived();
        public void KeyPress(string classID, MainPage mp)
        {
            string passValue = "";
            if (classID == "Enter")
            {
                passValue = "13";
            }
            else
            {
                var ascii_values = classID.Select(x => (int)x);
                foreach (var r in ascii_values)
                {
                    passValue = passValue + r;
                }

            }
            CharacterReceived.Receive(passValue, mp);
            if (passValue == "13")
            {
                mp.stkKeyboardCombo.IsVisible = false;
            }
        }
    }
}
