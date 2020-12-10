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
        Loading loadingPage;
        Database db;

		public CreateAccountPage ()
		{
			InitializeComponent();
            db = new Database();
            db.WarmupServer();
        }

        private void CreateAccount(object sender, EventArgs e)
        {
            CreateAccountWithSplashScreen();
        }

        private bool VerifyCreatedAccount()
        {
            if (username.Text != "" && password.Text != "")
            {
                if (new Database().UserRegister(username.Text, password.Text))
                {
                    App.Username = username.Text;
                    App.Password = password.Text;
                    return true;
                }
            }
            return false;
        }

        private async void CreateAccountWithSplashScreen()
        {
            loadingPage = new Loading(Loading.LoadType.CreatingAccount);
            await Navigation.PushModalAsync(loadingPage);
            loadingPage.IsLoading = true;

            await Task.Run(() =>
            {
                if (VerifyCreatedAccount())
                {
                    loadingPage.Result = true;
                    loadingPage.success = true;
                }
                else
                {
                    loadingPage.Result = false;
                    loadingPage.success = false;
                }
                loadingPage.IsLoading = false;
            });

            if (loadingPage.success)
            {
                if (db.UserLogin(App.Username, App.Password))
                {
                    Application.Current.MainPage = new MainPage();
                }
            }
            else
            {
                await Navigation.PopModalAsync();
                await DisplayAlert("Create Account Error!", "The credentials you entered are invalid. Please make sure you enter a Username and verify your Password", "OK");
            }
        }

        private void SignIn(object sender, EventArgs e)
        {
            Application.Current.MainPage = new LoginPage();
        }
    }
}