using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MobileApplication.Models;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MobileApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }
        ObservableCollection<Item> inventory;
        private string[] itemInfo = new string[5] { "", "", "", "", "" };
        Loading loadingPage;
        Database db;

        public NewItemPage(Ingredient ing)
        {
            InitializeComponent();
            GetItem(ing);
        }

        public NewItemPage()
        {
            InitializeComponent();
            GetItem();
        }

        public void GetItem()
        {
            GetItem(null);
        }

        private void GetItem(Ingredient ing)
        {
            db = new Database();            //Instantiate the database item to interact with the database
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
                UPC = "",
                ProductName = "",
                Description = "",
                ImageUrl = "",
                Quantity = ""
            };

            if (ing != null)
            {
                Item.ProductName = ing.text;
                Item.ImageUrl = ing.image;
            }

            BindingContext = this;

            QuantitySelector.ValueChanged += (sender, e) =>
            {
                Quantity.Text = e.NewValue.ToString();
            };
        }

        void Save_Clicked(object sender, EventArgs e)
        {
            if (App.editingItem)
            {
                db.RemoveFromUserInventory(App.Username, App.ScannedUPC);
                App.editingItem = false;
            }

            AddItemWithSplashScreen();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            App.editingItem = false;
            App.ScannedUPC = "";
            await Navigation.PopModalAsync();
        }

        public async void AddItemWithSplashScreen()
        {
            loadingPage = new Loading(Loading.LoadType.SavingInventory);
            await Navigation.PushModalAsync(loadingPage);
            loadingPage.IsLoading = true;

            await Task.Run(() =>
            {
                db = new Database();
                if (db.AddToUserInventory(App.Username, Item.UPC, Item.ProductName, Item.Description, Item.ImageUrl, int.Parse(Quantity.Text)))
                {
                    loadingPage.success = true;
                }
                else
                {
                    loadingPage.success = false;
                }
            });

            if (loadingPage.success)
            {
                Application.Current.MainPage = new MainPage();
            }
            else
            {
                await Navigation.PopModalAsync();
                await DisplayAlert("Inventory Save Failed", "Item " + itemInfo[1] + " not added to inventory", "OK");
            }
        }
    }
}