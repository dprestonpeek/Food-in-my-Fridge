using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApplication.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        Loading loadingPage;

        public LoginPage ()
		{
			InitializeComponent ();
        }

        void SignIn(object sender, EventArgs e)
        {
            LoginWithSplashScreen();
        }

        void CreateAccount(object sender, EventArgs e)
        {
            Application.Current.MainPage = new CreateAccountPage();
        }

        private bool VerifyLogin()
        {
            if (new Database().UserLogin(username.Text, password.Text))
            {
                App.Username = username.Text;
                App.Password = password.Text;
                return true;
            }
            loadingPage.IsLoading = false;
            return false;
        }

        private async void LoginWithSplashScreen()
        {
            loadingPage = new Loading(Loading.LoadType.LogginIn);
            await Navigation.PushModalAsync(loadingPage);
            loadingPage.IsLoading = true;

            await Task.Run(() =>
            {
                if (VerifyLogin())
                {
                    loadingPage.success = true;
                }
                else
                {
                    loadingPage.success = false;
                }
            });

            if (loadingPage.success)
            {
                Application.Current.MainPage = new MainPage();
            }
            else
            {
                await Navigation.PopModalAsync();
                await DisplayAlert("Username/Password not found", "The username and password combination you entered does not exist.", "OK");
            }
        }


    }
}