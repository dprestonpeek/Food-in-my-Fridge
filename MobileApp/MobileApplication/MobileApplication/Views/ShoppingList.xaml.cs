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

        public ShoppingList()
        {
            InitializeComponent();
        }

        private async void GetShoppingListWithSplashScreen(string username)
        {
            loadingPage = new Loading(Loading.LoadType.ShoppingList);
            await Navigation.PushModalAsync(loadingPage);
            loadingPage.IsLoading = true;
            ChooseShoppingList.IsVisible = false;
            shoppingListButtons.IsVisible = true;

            await Task.Run(() =>
            {
                if (db.UserExists(username))
                {
                    shoppingListButtons.Children.Clear();
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
                    foreach (Item shoppingListItem in (List<Item>)loadingPage.Result)
                    {
                        Button button = new Button { Text = shoppingListItem.ProductName, ImageSource = shoppingListItem.ImageUrl, }; //0 = UPC code 1 = product name 2= description 3 = image url 4 = quanity 
                        shoppingListButtons.Children.Add(button);
                        button.Clicked += OpenShoppingListItem;
                    }
                }
                await Navigation.PopModalAsync();
            }
            else
            {
                await Navigation.PopModalAsync();
                ChooseShoppingList.IsVisible = true;
                shoppingListButtons.IsVisible = false;
                await DisplayAlert("Shopping List not found", "Were the credentials typed correctly?", "OK");
            }
        }

        private void OpenMyShoppingList(object sender, EventArgs e)
        {
            GetShoppingListWithSplashScreen(App.Username);
        }

        private async void SpecifyUserShoppingList(object sender, EventArgs e)
        {
            string otherUsername = await DisplayPromptAsync("Shopping List to Display", "Enter a username to display another user's shopping list: ");
            if (db.UserExists(otherUsername))
            {
                GetShoppingListWithSplashScreen(otherUsername);
            }
        }

        private void OpenShoppingListItem(object sender, EventArgs e)
        {
            Item thisItem = null;
            Button button = (Button)sender;
            string buttonText = button.Text;

            foreach (Item shoppingListItem in shoppingList)
            {
                string shoppingListButtonLabel = shoppingListItem.ProductName;
                if (shoppingListButtonLabel == buttonText)
                {
                    thisItem = shoppingListItem;
                }
            }
            Navigation.PushModalAsync(new ShoppingListDetail(thisItem));
        }


        private void addToShoppingList(Item item)
        {
            // item.ProductName;
        }

    }
}