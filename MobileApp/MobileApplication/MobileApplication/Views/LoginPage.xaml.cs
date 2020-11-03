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
            OpenSplashScreen();
        }

        void CreateAccount(object sender, EventArgs e)
        {
            Application.Current.MainPage = new CreateAccountPage();
        }
        
        private async void OpenSplashScreen()
        {
            loadingPage = new Loading(Loading.LoadType.LogginIn, username.Text, password.Text);

            loadingPage.IsLoading = true;

            Device.BeginInvokeOnMainThread(() => {
                if (new Database().UserLogin(username.Text, password.Text))
                {
                    App.Username = username.Text;
                    App.Password = password.Text;
                    loadingPage.success = true;
                }
                else
                {
                    loadingPage.success = false;
                }
                loadingPage.IsLoading = false;
            });

            if (loadingPage.IsLoading)
            {
                await Navigation.PushModalAsync(loadingPage);
                while (loadingPage.IsLoading) { }
            }

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