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
	public partial class FrontPage : ContentPage
	{
		public FrontPage ()
		{
			InitializeComponent ();
		}

        void NaviagteToLoginPage(object sender, EventArgs e)
        {
            Application.Current.MainPage = new LoginPage();

        }
        void NaviagteToCreateAccountPage(object sender, EventArgs e)
        {
            Application.Current.MainPage = new CreateAccountPage();

        }
    }
}