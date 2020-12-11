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
	public partial class ShoppingListDetail : ContentPage
	{
        List<Item> shoppingList;
        Item shoppingListItem;
        Loading loadingPage;
        Database db;

        public ShoppingListDetail (Item shoppingListItem, List<Item> shoppingList)
		{
			InitializeComponent ();
            db = new Database();
            this.shoppingListItem = shoppingListItem;
            this.shoppingList = shoppingList;
            ProductName.Text = shoppingListItem.ProductName;
            ProductImage.Source = shoppingListItem.ImageUrl;
            ProductDesc.Text = shoppingListItem.Description;
            ProductUPC.Text = shoppingListItem.UPC;
            ProductQuantity.Text = shoppingListItem.Quantity;
        }

        private void DeleteItem(object sender, EventArgs e)
        {
            DeleteItemWithSplashScreen(App.Username, shoppingList);
        }

        private async void DeleteItemWithSplashScreen(string username, List<Item> theShoppingList)
        {
            loadingPage = new Loading(Loading.LoadType.ShoppingListSave);
            await Navigation.PushModalAsync(loadingPage);
            loadingPage.IsLoading = true;

            await Task.Run(() =>
            {
                if (db.RemoveFromShoppingList(shoppingListItem.UPC))
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
                Application.Current.MainPage = new NavigationPage(new ShoppingList());
            }
            else
            {
                await Navigation.PopModalAsync();
                await DisplayAlert("Shopping List Error!", "Unable to remove " + shoppingListItem.ProductName + " from shopping list.", "OK");
            }
        }
    }
}