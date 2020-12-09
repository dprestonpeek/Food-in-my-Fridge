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
        Database db = new Database();
        List <ShoppingListItem> shoppingList;

        public ShoppingList()
        {
            InitializeComponent();
            getShoppingList();
        }

        private void getShoppingList()
        {
            shoppingList = db.GetUserShoppingList();
            listStack.Children.Clear();
            foreach (ShoppingListItem shoppingListItem in shoppingList)
            {
                Button button = new Button { Text = shoppingListItem.Name, ImageSource = shoppingListItem.ImageUrl, }; //0 = UPC code 1 = product name 2= description 3 = image url 4 = quanity 
                listStack.Children.Add(button);
                button.Clicked += OpenShoppingListItem;
            }
        }

        private void OpenShoppingListItem(object sender, EventArgs e)
        {
            ShoppingListItem thisItem = null;
            Button button = (Button)sender;
            string buttonText = button.Text;

            foreach (ShoppingListItem shoppingListItem in shoppingList)
            {
                string shoppingListButtonLabel = shoppingListItem.Name;
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