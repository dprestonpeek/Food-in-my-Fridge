using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using MobileApplication.Views;
using MobileApplication.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecipesPage : ContentPage
    {
        string searchInput;
        Database db;
        private Loading loadingPage;

        public RecipesPage()
        {
            InitializeComponent();
            db = new Database();
        }

        private async void GetRecipesWithSplashScreen()
        {
            loadingPage = new Loading(Loading.LoadType.SearchingRecipes);
            await Navigation.PushModalAsync(loadingPage);
            loadingPage.IsLoading = true;

            List<Recipe> recipes = null;
            RecipeList recipeList = null;

            await Task.Run(() =>
            {
                recipes = db.products.GetRecipes(searchInput);
                if (recipes.Count > 0)
                {
                    loadingPage.Result = recipes;
                    loadingPage.success = true;
                    recipeList = new RecipeList(recipes, new RecipesViewModel(recipes));
                }
                else
                {
                    loadingPage.success = false;
                }
            });

            if (loadingPage.success)
            {
                await Navigation.PushModalAsync(recipeList);
            }
            else
            {
                await Navigation.PopModalAsync();
                await DisplayAlert("No Recipes Found", "There were no recipes found based on this keyword.", "OK");
            }
        }

        //on btn clicked
        public void SearchBtn_Clicked(object sender, EventArgs e)
        {
            searchInput = item.Text; //get user input from xaml entry
            GetRecipesWithSplashScreen();

            //this will find recipes by keyword - does not look at inventory

            //List<Recipe> recipes = db.products.GetRecipes(searchInput); //gets list of recipe results
            //Recipe - (string) label, source, image, calories, url, (int) time, servings, List<Ingredient> ingredients
            //Ingredient - string parentNode, text, weight, image

            //GetProductData() method retuns string array - [0]barcode number [1]product name [2]description [3]image url [4]quantity




            //results - when set up to return multiple results, go through this code for every (maxRecipes = 10) result
            //string res_label = recipes[0].Label;
            //string res_source = recipes[0].source;
            //string res_img = recipes[0].Image;
            //string res_calories = recipes[0].calories;
            //string res_url = recipes[0].url; //need to attatch this to label hyperlink "See full recipe"

            //string ingr_1 = recipes[0].ingredients[0].text;  //gets the text of the first ingredient from the first recipe



            /*/display results
            recipeName.Text = "LABEL:" + res_label + "\tCAL: " + res_calories;
            recipeDesc.Text = "SOURCE: " + res_source;
            recipeUrl.Text = res_url;
            recipeImg.Source = res_img;
            ingr.Text = "INGR: " + ingr_1;*/

            //Items = new ObservableCollection<Recipe>();

            //foreach (var rec in recipes)
            //{
            //    Recipe recipe = new Recipe()
            //    {
            //        Image = rec.Image,
            //        Label = rec.Label,
            //        Source = rec.Source,
            //        //calories = rec.calories,
            //        //servings = rec.servings,
            //        //time = rec.time,
            //        //dietLabels = rec.dietLabels,
            //        //healthLabels = rec.healthLabels,
            //        //ingredients = rec.ingredients,
            //        //url = rec.url
            //    };
            //    Items.Add(recipe);
            //}
            //RecipeList.ItemsSource = Items;

        }
        public void OpenlinkBtn_Clicked(object sender, EventArgs e)
        {
            //want to be able to open link to recipe webpage in browser
        }
        public void SuggestBtn_Clicked(object sender, EventArgs e)
        {
            //want to be able to suggest recipes by comparing to names of items in inventory 'fridge' to recipe ingredients
        }
    }
}
