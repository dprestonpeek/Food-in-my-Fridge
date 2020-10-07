using MobileApplication.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApplication
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ExamplePage : ContentPage
	{
		public ExamplePage ()
		{
			InitializeComponent ();
		}

        //This is how you change pages when no actions need to be completed.
        private void AboutButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AboutPage());
        }
        //Use PushModalAsync when there is an action that needs to be completed. Like a barcode scan or a popup text box

        //This is how you navigate home
        private void HomeButton_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new MainPage();
        }

        //to add pages to main menu edit these 3 files
        //HomeMenuItem.cs > MenuItemType 
        //MainPage.xaml.cs > NavigateFromMenu(int)
        //MenuPage.xaml.cs > menuitems
    }
}