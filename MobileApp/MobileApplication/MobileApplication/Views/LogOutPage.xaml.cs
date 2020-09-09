using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApplication.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LogOutPage : ContentPage
	{
		public LogOutPage ()
		{
			InitializeComponent ();
            YesLogout.Clicked += Logout;
            NoStayLoggedIn.Clicked += StayLoggedIn;
		}

        private void StayLoggedIn(object sender, EventArgs e)
        {
            Application.Current.MainPage = new MainPage();
        }

        private void Logout(object sender, EventArgs e)
        {
            App.Username = "";
            App.Password = "";
            Application.Current.MainPage = new LoginPage();
        }
    }
}