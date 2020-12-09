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
        Item shoppingListItem;

        public ShoppingListDetail (Item shoppingListItem)
		{
			InitializeComponent ();
            this.shoppingListItem = shoppingListItem;
            shoppingListName.Text = shoppingListItem.ProductName;
            shoppingListImage.Source = shoppingListItem.ImageUrl;
            shoppingListDescription.Text = shoppingListItem.Description;
            shoppingListQuanity.Text = shoppingListItem.Quantity;
        }
	}
}