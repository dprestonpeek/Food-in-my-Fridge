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
            //this process will find recipes by keyword - does not look at inventory

            Database db = new Database();   //create database
            string searchInput = item.Text; //get user input from xaml entry

            recipes = db.products.GetRecipes(searchInput);  //gets list of recipe results

            if (recipes[0].Source == "")
            {
                DisplayAlert("Error!", recipes[0].Label, "OK");
                return;
            }

            //results - when set up to return multiple results, go through this code for every (maxRecipes = 10) result
            string res_label = recipes[0].Label;
            string res_source = recipes[0].Source;
            string res_img = recipes[0].Image;
            string res_calories = recipes[0].Calories;
            string res_url = recipes[0].Url; //need to attatch this to label hyperlink "See full recipe"

            string ingr_1 = recipes[0].Ingredients[0].text;  //gets the text of the first ingredient from the first recipe

            LabelStack.Children.Clear();
            foreach(var rec in recipes)
            {
                Button button = new Button { Text = rec.Label + "\nCalories: " + rec.Calories, ImageSource = rec.Image };
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
                string recipeBtnLabel = recipe.Label + "\nCalories: " + recipe.Calories;
                if (recipeBtnLabel == buttonText)
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
