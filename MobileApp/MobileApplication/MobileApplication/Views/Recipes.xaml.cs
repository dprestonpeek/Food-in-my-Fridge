using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Recipes : ContentPage
    {
        List<Recipe> recipes;

        public Recipes()
        {
            InitializeComponent();
        }

        //on btn clicked
        public void SearchBtn_Clicked(object sender, EventArgs e)
        {
            Database db = new Database();   //create database

            string searchInput = item.Text; //get user input from xaml entry

            //this will find recipes by keyword - does not look at inventory

            recipes = db.products.GetRecipes(searchInput); //gets list of recipe results
            //Recipe - (string) label, source, image, calories, url, (int) time, servings, List<Ingredient> ingredients
            //Ingredient - string parentNode, text, weight, image

            //GetProductData() method retuns string array - [0]barcode number [1]product name [2]description [3]image url [4]quantity




            //results - when set up to return multiple results, go through this code for every (maxRecipes = 10) result
            string res_label = recipes[0].Label;
            string res_source = recipes[0].Source;
            string res_img = recipes[0].Image;
            string res_calories = recipes[0].Calories;
            string res_url = recipes[0].Url; //need to attatch this to label hyperlink "See full recipe"

            string ingr_1 = recipes[0].Ingredients[0].text;  //gets the text of the first ingredient from the first recipe



            /*/display results
            recipeName.Text = "LABEL:" + res_label + "\tCAL: " + res_calories;
            recipeDesc.Text = "SOURCE: " + res_source;
            recipeUrl.Text = res_url;
            recipeImg.Source = res_img;
            ingr.Text = "INGR: " + ingr_1;*/

            LabelStack.Children.Clear();
            foreach(var rec in recipes)
            {
                Button button = new Button { Text = rec.Label, ImageSource = rec.Image };
                LabelStack.Children.Add(button);
                button.Clicked += OpenRecipePage;
            }
        }

        public void OpenRecipePage(object sender, EventArgs e)
        {
            Recipe thisRecipe = null;
            Button button = (Button)sender;
            string buttonText = button.Text;

            foreach (Recipe recipe in recipes)
            {
                if (recipe.Label == buttonText)
                {
                    thisRecipe = recipe;
                }
            }

            Navigation.PushModalAsync(new RecipeDetailPage(thisRecipe));
        }

        public void SuggestBtn_Clicked(object sender, EventArgs e)
        {
            //want to be able to suggest recipes by comparing to names of items in inventory 'fridge' to recipe ingredients
        }
    }
}