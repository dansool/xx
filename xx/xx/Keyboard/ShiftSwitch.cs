using System;
using System.Collections.Generic;
using System.Text;

namespace xx.Keyboard
{
    public class ShiftSwitch
    {
        public void Switch(bool isCapital, MainPage mp)
        {
            if (isCapital)
            {
                mp.btnKeyboardQ.Text = "q";
                mp.btnKeyboardW.Text = "w";
                mp.btnKeyboardE.Text = "e";
                mp.btnKeyboardR.Text = "r";
                mp.btnKeyboardT.Text = "t";
                mp.btnKeyboardY.Text = "y";
                mp.btnKeyboardU.Text = "u";
                mp.btnKeyboardI.Text = "i";
                mp.btnKeyboardO.Text = "o";
                mp.btnKeyboardP.Text = "p";
                mp.btnKeyboardÜ.Text = "ü";
                mp.btnKeyboardA.Text = "a";
                mp.btnKeyboardS.Text = "s";
                mp.btnKeyboardD.Text = "d";
                mp.btnKeyboardF.Text = "f";
                mp.btnKeyboardG.Text = "g";
                mp.btnKeyboardH.Text = "h";
                mp.btnKeyboardJ.Text = "j";
                mp.btnKeyboardK.Text = "k";
                mp.btnKeyboardL.Text = "l";
                mp.btnKeyboardÕ.Text = "õ";
                mp.btnKeyboardÄ.Text = "ä";
                mp.btnKeyboardZ.Text = "z";
                mp.btnKeyboardX.Text = "x";
                mp.btnKeyboardC.Text = "c";
                mp.btnKeyboardV.Text = "v";
                mp.btnKeyboardB.Text = "b";
                mp.btnKeyboardN.Text = "n";
                mp.btnKeyboardM.Text = "m";
                mp.btnKeyboardÖ.Text = "ö";
            }
            else
            {
                mp.btnKeyboardQ.Text = "Q";
                mp.btnKeyboardW.Text = "W";
                mp.btnKeyboardE.Text = "E";
                mp.btnKeyboardR.Text = "R";
                mp.btnKeyboardT.Text = "T";
                mp.btnKeyboardY.Text = "Y";
                mp.btnKeyboardU.Text = "U";
                mp.btnKeyboardI.Text = "I";
                mp.btnKeyboardO.Text = "O";
                mp.btnKeyboardP.Text = "P";
                mp.btnKeyboardÜ.Text = "Ü";
                mp.btnKeyboardA.Text = "A";
                mp.btnKeyboardS.Text = "S";
                mp.btnKeyboardD.Text = "D";
                mp.btnKeyboardF.Text = "F";
                mp.btnKeyboardG.Text = "G";
                mp.btnKeyboardH.Text = "H";
                mp.btnKeyboardJ.Text = "J";
                mp.btnKeyboardK.Text = "K";
                mp.btnKeyboardL.Text = "L";
                mp.btnKeyboardÕ.Text = "Õ";
                mp.btnKeyboardÄ.Text = "Ä";
                mp.btnKeyboardZ.Text = "Z";
                mp.btnKeyboardX.Text = "X";
                mp.btnKeyboardC.Text = "C";
                mp.btnKeyboardV.Text = "V";
                mp.btnKeyboardB.Text = "B";
                mp.btnKeyboardN.Text = "N";
                mp.btnKeyboardM.Text = "M";
                mp.btnKeyboardÖ.Text = "Ö";
            }
        }

        public void SwitchAdv(bool isRegular, MainPage mp)
        {
            if (isRegular)
            {
                mp.stkKeyboardAdvAt.IsVisible = false;
                mp.stkKeyboardAdvHash.IsVisible = false;
                mp.stkKeyboardAdvDollar.IsVisible = false;
                mp.stkKeyboardAdvPercent.IsVisible = false;
                mp.stkKeyboardAdvAmp.IsVisible = false;
                mp.stkKeyboardAdvAsterix.IsVisible = false;
                mp.stkKeyboardAdvStartCap.IsVisible = false;
                mp.stkKeyboardAdvCloseCap.IsVisible = false;
                mp.stkKeyboardAdvDash.IsVisible = false;
                mp.stkKeyboardAdvBackSlash.IsVisible = false;
                mp.stkKeyboardAdvExclamation.IsVisible = false;
                mp.stkKeyboardAdvSemicolon.IsVisible = false;
                mp.stkKeyboardAdvColon.IsVisible = false;
                mp.stkKeyboardAdvUpperComma.IsVisible = false;
                mp.stkKeyboardAdvQuatation.IsVisible = false;
                mp.stkKeyboardAdvQuestionMark.IsVisible = false;
                mp.stkKeyboardAdvSlash.IsVisible = false;

                mp.stkKeyboardAdvUpperBigger.IsVisible = true;
                mp.stkKeyboardAdvBracketOpen.IsVisible = true;
                mp.stkKeyboardAdvBracketClose.IsVisible = true;
                mp.stkKeyboardAdvCurlyOpen.IsVisible = true;
                mp.stkKeyboardAdvCurlyClose.IsVisible = true;
                mp.stkKeyboardAdvSmallerthan.IsVisible = true;
                mp.stkKeyboardAdvGreaterthan.IsVisible = true;
                mp.stkKeyboardAdvEuro.IsVisible = true;
                mp.stkKeyboardAdvPound.IsVisible = true;
                mp.stkKeyboardAdvYen.IsVisible = true;
                mp.stkKeyBoardAdvBigDot.IsVisible = true;
                mp.stkKeyboardAdvDash2.IsVisible = true;
                mp.stkKeyboardAdvPlus.IsVisible = true;
                mp.stkKeyboardAdvEqual.IsVisible = true;
                mp.stkKeyboardAdvUnderScore.IsVisible = true;
                mp.stkKeyboardAdvAbout.IsVisible = true;
                mp.stkKeyboardVerticalDash.IsVisible = true;

            }
            else
            {
                mp.stkKeyboardAdvAt.IsVisible = true;
                mp.stkKeyboardAdvHash.IsVisible = true;
                mp.stkKeyboardAdvDollar.IsVisible = true;
                mp.stkKeyboardAdvPercent.IsVisible = true;
                mp.stkKeyboardAdvAmp.IsVisible = true;
                mp.stkKeyboardAdvAsterix.IsVisible = true;
                mp.stkKeyboardAdvStartCap.IsVisible = true;
                mp.stkKeyboardAdvCloseCap.IsVisible = true;
                mp.stkKeyboardAdvDash.IsVisible = true;
                mp.stkKeyboardAdvBackSlash.IsVisible = true;
                
                mp.stkKeyboardAdvExclamation.IsVisible = true;
                mp.stkKeyboardAdvSemicolon.IsVisible = true;
                mp.stkKeyboardAdvColon.IsVisible = true;
                mp.stkKeyboardAdvUpperComma.IsVisible = true;
                mp.stkKeyboardAdvQuatation.IsVisible = true;
                mp.stkKeyboardAdvQuestionMark.IsVisible = true;
                mp.stkKeyboardAdvSlash.IsVisible = true;

                mp.stkKeyboardAdvUpperBigger.IsVisible = false;
                mp.stkKeyboardAdvBracketOpen.IsVisible = false;
                mp.stkKeyboardAdvBracketClose.IsVisible = false;
                mp.stkKeyboardAdvCurlyOpen.IsVisible = false;
                mp.stkKeyboardAdvCurlyClose.IsVisible = false;
                mp.stkKeyboardAdvSmallerthan.IsVisible = false;
                mp.stkKeyboardAdvGreaterthan.IsVisible = false;
                mp.stkKeyboardAdvEuro.IsVisible = false;
                mp.stkKeyboardAdvPound.IsVisible = false;
                mp.stkKeyboardAdvYen.IsVisible = false;


                mp.stkKeyBoardAdvBigDot.IsVisible = false;
                mp.stkKeyboardAdvDash2.IsVisible = false;
                mp.stkKeyboardAdvPlus.IsVisible = false;
                mp.stkKeyboardAdvEqual.IsVisible = false;
                mp.stkKeyboardAdvUnderScore.IsVisible = false;
                mp.stkKeyboardAdvAbout.IsVisible = false;
                mp.stkKeyboardVerticalDash.IsVisible = false;

            }
        }
    }
}
