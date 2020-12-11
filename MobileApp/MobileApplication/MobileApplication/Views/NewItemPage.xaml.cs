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
        private string[] itemInfo = new string[5] { "", "", "", "", "" };
        Loading loadingPage;
        Database db;
        bool shoppingListItem = false;
        List<Item> shoppingList;

        public NewItemPage(Ingredient ing)
        {
            InitializeComponent();
            GetItem(ing);
        }

        public NewItemPage(List<Item> shoppingList)
        {
            InitializeComponent();
            shoppingListItem = true;
            this.shoppingList = shoppingList;
            GetItem(null);
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
            db = new Database();

            if (App.editingItem)
            {
                Title = "Edit Item";
                itemInfo = db.GetItemFromInventory(App.Username, App.ScannedUPC);
            }
            else
            {
                Title = "New Item";
                if (App.ScannedUPC != "")
                {
                    itemInfo = db.GetProductDataByUPC(App.ScannedUPC);
                }
                else
                {
                    itemInfo[0] = "";
                    itemInfo[1] = "";
                    itemInfo[2] = "";
                    itemInfo[3] = "";
                    itemInfo[4] = "1";
                }
            }

            Item = new Item
            {
                UPC = itemInfo[0],
                ProductName = itemInfo[1],
                Description = itemInfo[2],
                ImageUrl = itemInfo[3],
                Quantity = itemInfo[4]
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
            if (shoppingListItem)
            {
                if (App.editingItem)
                {
                    db.RemoveFromShoppingList(App.ScannedUPC);
                    App.editingItem = false;
                }

                AddItemWithSplashScreen();
            }
            else
            {
                if (App.editingItem)
                {
                    db.RemoveFromUserInventory(App.Username, App.ScannedUPC);
                    App.editingItem = false;
                }

                AddItemWithSplashScreen();
            }
        }

        void SaveCustomUrl_Clicked(object sender, EventArgs e)
        {
            Item.ImageUrl = CustomImageUrl.Text;
            ImageButton.ImageSource = Item.ImageUrl;
            ImageSelection.IsVisible = false;
            ImageDisplay.IsVisible = true;
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            App.editingItem = false;
            App.ScannedUPC = "";
            await Navigation.PopModalAsync();
        }

        void CancelImageChange_Clicked(object sender, EventArgs e)
        {
            ImageSelection.IsVisible = false;
            ImageDisplay.IsVisible = true;
        }

        async void Image_Clicked(object sender, EventArgs e)
        {

            string searchTerm = await DisplayPromptAsync("Edit Product Image", "Search for images based on keyword: ");
            if (searchTerm == "")
            {
                searchTerm = Item.ProductName;
            }
            ImageSelection.IsVisible = true;
            ImageDisplay.IsVisible = false;

            string[] imageNames = new string[6];
            try
            {
                imageNames = db.GetProductImagesByKeyword(searchTerm);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", ex.Message, "OK");
                ImageSelection.IsVisible = false;
                ImageDisplay.IsVisible = true;
            }
            
            Image1.Source = imageNames[0];
            Image2.Source = imageNames[1];
            Image3.Source = imageNames[2];
            Image4.Source = imageNames[3];
            Image5.Source = imageNames[4];
            Image6.Source = imageNames[5];
        }

        void NewImageSelected(object sender, EventArgs e)
        {
            Button clicked = (Button)sender;
            Item.ImageUrl = GetSelectedImageSource(clicked);
            ImageButton.ImageSource = Item.ImageUrl;
            ImageSelection.IsVisible = false;
            ImageDisplay.IsVisible = true;
        }

        string GetSelectedImageSource(Button clicked)
        {
            switch(clicked.Text)
            {
                case "1":
                    return Image1.Source.ToString().Substring(5);
                case "2":
                    return Image2.Source.ToString().Substring(5);
                case "3":
                    return Image3.Source.ToString().Substring(5);
                case "4":
                    return Image4.Source.ToString().Substring(5);
                case "5":
                    return Image5.Source.ToString().Substring(5);
                case "6":
                    return Image6.Source.ToString().Substring(5);
                default:
                    return "Image Unavailable.";
            }
        }

        public async void AddItemWithSplashScreen()
        {
            if (shoppingListItem)
            {
                loadingPage = new Loading(Loading.LoadType.ShoppingListSave);
            }
            else
            {
                loadingPage = new Loading(Loading.LoadType.SavingInventory);
            }
            await Navigation.PushModalAsync(loadingPage);
            loadingPage.IsLoading = true;

            await Task.Run(() =>
            {
                db = new Database();
                if (!App.editingItem)
                {
                    if (QuantitySelector.Value == 0)
                    {
                        QuantitySelector.Value = 1;
                    }
                }
                Item.Quantity = QuantitySelector.Value.ToString();
                if (shoppingListItem)
                {
                    if (db.AddItemToShoppingList(Item))
                    {
                        loadingPage.success = true;
                    }
                    else
                    {
                        loadingPage.success = false;
                    }
                }
                else
                {
                    if (db.AddToUserInventory(Item))
                    {
                        loadingPage.success = true;
                    }
                    else
                    {
                        loadingPage.success = false;
                    }
                }
            });

            if (shoppingListItem)
            {
                if (loadingPage.success)
                {
                    await Navigation.PopModalAsync();
                    await Navigation.PopModalAsync();
                }
                else
                {
                    await Navigation.PopModalAsync();
                    await DisplayAlert("Shopping List Save Failed", "Item " + itemInfo[1] + " not added to shopping list", "OK");
                }
            }
            else
            {
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
}