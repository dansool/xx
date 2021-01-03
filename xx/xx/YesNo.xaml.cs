using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;

namespace xx
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class YesNo : ContentPage
	{
        MainPage mp = new MainPage();
        //public bool YesNoResult = false;

        public  YesNo (MainPage mpFromMainPage)
		{
            mp = mpFromMainPage;
            InitializeComponent ();
		}

        private async void btnNo_Pressed(object sender, EventArgs e)
        {            
            //YesNoResult = false;
            mp.YesNoResult = false;
            
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void btnYes_Pressed(object sender, EventArgs e)
        {
            //YesNoResult = true;
            mp.YesNoResult = true;
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}