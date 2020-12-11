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
	public partial class EditShoppingList : ContentPage
	{
        List<Item> shoppingList;
        Database db;

		public EditShoppingList (List<Item> shoppingList)
		{
			InitializeComponent ();
            this.shoppingList = shoppingList;
            db = new Database();

            foreach (Item shoppingListItem in shoppingList)
            {
                Button button = new Button { Text = shoppingListItem.ProductName, ImageSource = shoppingListItem.ImageUrl, }; //0 = UPC code 1 = product name 2= description 3 = image url 4 = quanity 
                shoppingListButtons.Children.Add(button);
                button.Clicked += OpenShoppingListItem;
            }
        }

        private void AddToShoppingList(object sender, EventArgs e)
        {
            NewItemPage newItemPage = new NewItemPage(shoppingList);
            Navigation.PushModalAsync(newItemPage);
            db.AddItemToShoppingList(newItemPage.Item);
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
            Navigation.PushModalAsync(new ShoppingListDetail(thisItem, shoppingList));
        }

        private void DoneEditing(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ShoppingListView(shoppingList, true));
        }
    }
}