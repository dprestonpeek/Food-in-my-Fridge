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
	public partial class CreateAccountPage : ContentPage
	{
		public CreateAccountPage ()
		{
			InitializeComponent ();
            
		}

        private void CreateAccount(object sender, EventArgs e)
        {
            Database db = new Database();
            if (username.Text != "" && password.Text != "" && !db.UserLogin(username.Text, password.Text))
            {
                if (db.UserRegister(username.Text, password.Text))
                {
                    App.Username = username.Text;
                    App.Password = password.Text;
                    Application.Current.MainPage = new MainPage();
                }
            }
        }

        private void SignIn(object sender, EventArgs e)
        {
            Application.Current.MainPage = new LoginPage();
        }
    }
}