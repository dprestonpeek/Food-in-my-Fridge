using MobileApplication.Models;
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
    public partial class ShoppingList : ContentPage
    {
        Loading loadingPage;
        Database db = new Database();
        List <Item> shoppingList;
        bool userOwnsThisList = false;

        public ShoppingList()
        {
            InitializeComponent();
        }

        public ShoppingList(List<Item> shoppingList)
        {
            InitializeComponent();
        }

        private void OpenMyShoppingList(object sender, EventArgs e)
        {
            userOwnsThisList = true;
            GetShoppingListWithSplashScreen(App.Username);
        }

        private async void SpecifyUserShoppingList(object sender, EventArgs e)
        {
            userOwnsThisList = false;
            string otherUsername = await DisplayPromptAsync("Shopping List to Display", "Enter a username to display another user's shopping list: ");
            if (db.UserExists(otherUsername))
            {
                GetShoppingListWithSplashScreen(otherUsername);
            }
        }

        private async void GetShoppingListWithSplashScreen(string username)
        {
            loadingPage = new Loading(Loading.LoadType.ShoppingListGet);
            await Navigation.PushModalAsync(loadingPage);
            loadingPage.IsLoading = true;

            await Task.Run(() =>
            {
                if (db.UserExists(username))
                {
                    shoppingList = db.GetUserShoppingList(username);

                    if (shoppingList != null)
                    {
                        loadingPage.Result = shoppingList;
                        loadingPage.success = true;
                    }
                    else
                    {
                        loadingPage.Result = false;
                        loadingPage.success = false;
                    }
                }
                loadingPage.IsLoading = false;
            });

            if (loadingPage.success)
            {
                if (loadingPage.Result != null)
                {
                    await Navigation.PushAsync(new ShoppingListView(shoppingList, userOwnsThisList));
                }
                await Navigation.PopModalAsync();
            }
            else
            {
                await Navigation.PopModalAsync();
                await DisplayAlert("Shopping List not found", "Were the credentials typed correctly?", "OK");
            }
        }
    }
}