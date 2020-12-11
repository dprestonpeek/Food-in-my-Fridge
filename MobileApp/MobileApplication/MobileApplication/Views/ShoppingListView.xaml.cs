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
	public partial class ShoppingListView : ContentPage
	{
        bool userOwnsThisList = false;

        List<Item> shoppingList;
		public ShoppingListView (List<Item> shoppingList, bool userOwnsList)
		{
			InitializeComponent ();
            this.shoppingList = shoppingList;
            userOwnsThisList = userOwnsList;

            shoppingListButtons.Children.Clear();
            shoppingListButtons.IsVisible = true;

            foreach (Item shoppingListItem in shoppingList)
            {
                Button button = new Button { Text = shoppingListItem.ProductName, ImageSource = shoppingListItem.ImageUrl, BackgroundColor = Color.LightGray}; //0 = UPC code 1 = product name 2= description 3 = image url 4 = quanity 
                shoppingListButtons.Children.Add(button);
                button.Clicked += CheckItem;
            }
        }

        private void CheckItem(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button.BackgroundColor == Color.LightGray)
            {
                button.BackgroundColor = Color.ForestGreen;
            }
            else if (button.BackgroundColor == Color.ForestGreen)
            {
                button.BackgroundColor = Color.LightGray;
            }
        }

        private async void EditButtonClicked(object sender, EventArgs e)
        {
            if (userOwnsThisList)
            {
                await Navigation.PushAsync(new EditShoppingList(shoppingList));
            }
            else
            {
                await DisplayAlert("Cannot Edit Shopping List!", "You cannot edit a shopping list that doesn't belong to you.", "OK");
            }
        }
    }
}