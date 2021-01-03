using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace xx.Keyboard
{
    public class CharacterReceived
    {
        public void Receive(string keyCode, MainPage mp)
        {
            try
            {
                switch (keyCode)
                {
                    case "13":
                        {
                            Debug.WriteLine("ENTER PRESSED!");
                            break;
                        }
                    case "8":
                        {
                            Entry focusedEditor = mp.FindByName<Entry>(mp.focusedEditor);
                            focusedEditor.Text = focusedEditor.Text.Length > 0 ? focusedEditor.Text.Remove(focusedEditor.Text.Length -1) : "";
                            break;
                        }
                    default:
                        {
                            char receivedChar = Convert.ToChar(Convert.ToInt32(keyCode));
                            Entry focusedEditor = mp.FindByName<Entry>(mp.focusedEditor);
                            focusedEditor.Text = focusedEditor.Text + receivedChar.ToString();
                            break;
                        }

                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
