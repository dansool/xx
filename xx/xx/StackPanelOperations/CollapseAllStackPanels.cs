using System;
using System.Collections.Generic;
using System.Text;

namespace xx.StackPanelOperations
{
    public class CollapseAllStackPanels
    {
        public void Collapse(xx.MainPage mp)
        {
            mp.stkUpdate.IsVisible = false;
            mp.stkEsimene.IsVisible = false;
            mp.stkTeine.IsVisible = false;
        }
    }
}
