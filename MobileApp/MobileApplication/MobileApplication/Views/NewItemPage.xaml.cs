using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MobileApplication.Models;
using System.Threading.Tasks;

namespace MobileApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }
        private string[] itemInfo = new string[5] { "", "", "", "", "" };
        Database db;

        public NewItemPage()
        {
            InitializeComponent();
            db = new Database();
            if (App.ScannedUPC != "")
            {
                if (App.editingItem)
                {
                    Title = "Edit Item";
                    itemInfo = db.GetItemFromInventory(App.Username, App.ScannedUPC);
                }
                else
                {
                    Title = "New Item";
                    itemInfo = db.products.GetProductData(App.ScannedUPC);
                }
            }
            else
            {
                itemInfo[0] = "";
                itemInfo[1] = "";
                itemInfo[2] = "";
                itemInfo[3] = "";
                itemInfo[4] = "";
            }
            Item = new Item
            {
                UPC = itemInfo[0],
                ProductName = itemInfo[1],
                Description = itemInfo[2],
                ImageUrl = itemInfo[3],
                Quantity = itemInfo[4]
            };

            BindingContext = this;

            QuantitySelector.ValueChanged += (sender, e) =>
            {
                Quantity.Text = e.NewValue.ToString();
            };
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            if (App.editingItem)
            {
                db.RemoveFromUserInventory(App.Username, App.ScannedUPC);
                App.editingItem = false;
            }
            if (!db.AddToUserInventory(App.Username, Item.UPC, Item.ProductName, Item.Description, Item.ImageUrl, int.Parse(Quantity.Text)))
            {
                await DisplayAlert("Error! Item not added to inventory", itemInfo[1], "OK");
            }
            Application.Current.MainPage = new MainPage();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            //App.Inventory = new string[4];
            App.editingItem = false;
            App.ScannedUPC = "";
            await Navigation.PopModalAsync();
        }
    }
}