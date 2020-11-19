using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MobileApplication.Models;
using MobileApplication.ViewModels;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MobileApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel viewModel;
        List<string> inventoryItems;
        Loading loadingPage;
        Database db;

        public ItemDetailPage(ItemDetailViewModel viewModel, ObservableCollection<Item> inventoryItems)
        {
            InitializeComponent();
            db = new Database();
            ButtonEditItem.Clicked += ButtonEditItem_Clicked;
            ButtonDeleteItem.Clicked += ButtonDeleteItem_Clicked;
            BindingContext = this.viewModel = viewModel;

            this.inventoryItems = new List<string>();
            foreach (Item item in inventoryItems)
            {
                this.inventoryItems.Add(item.ProductName);
            }
        }

        public ItemDetailPage()
        {
            InitializeComponent();
            var item = new Item
            {
                ProductName = "Item 1",
                Description = "This is an item description.",
            };

            viewModel = new ItemDetailViewModel(item);
            BindingContext = viewModel;
        }

        private void ButtonEditItem_Clicked(object sender, EventArgs e)
        {
            App.editingItem = true;
            App.ScannedUPC = viewModel.Item.UPC;
            Navigation.PushModalAsync(new NewItemPage());
        }

        private async void ButtonDeleteItem_Clicked(object sender, EventArgs e)
        {
            bool delete = await DisplayAlert("Delete Item", "Are you sure you want to delete this item?", "Yes", "No");
            if (delete)
            {
                DeleteItemWithSplashScreen();
            }
        }

        public async void DeleteItemWithSplashScreen()
        {
            loadingPage = new Loading(Loading.LoadType.SavingInventory);
            await Navigation.PushModalAsync(loadingPage);
            loadingPage.IsLoading = true;

            await Task.Run(() =>
            {
                if (db.RemoveFromUserInventory(App.Username, viewModel.Item.UPC))
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
                await DisplayAlert("Inventory Save Failed", "There was a problem saving your inventory.", "OK");
            }
        }

        private void SuggestRecipes_Clicked(object sender, EventArgs e)
        {
            List<Ingredient> recipeIng = new List<Ingredient>();
            List<string> ingredients = new List<string>();
            List<Recipe> recipes = db.GetRecipes(viewModel.Item.ProductName, 3);

            foreach (Recipe recipe in recipes)
            {
                foreach (Ingredient ing in recipe.Ingredients)
                {
                    ingredients.Add(ing.text);
                }
                recipe.score = RecipeMatch.GetRecipeScore(new List<string>() { viewModel.Item.ProductName }, ingredients);
            }

            if (recipes.Count > 0)
            {
                recipes.Sort((x, y) => x.score.CompareTo(y.score));
            }

            Navigation.PushModalAsync(new SuggestedRecipes(recipes));
        }
    }
}