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

            recipes = db.GetRecipes(searchInput);  //gets list of recipe results

            if (recipes[0].Source == "")
            {
                DisplayAlert("Error!", recipes[0].Label, "OK");
                return;
            }

            LabelStack.Children.Clear();
            foreach(var rec in recipes)
            {
                Button button = new Button { Text = rec.Label + "\nServings: " + rec.Servings, ImageSource = rec.Image };
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
                string recipeBtnLabel = recipe.Label + "\nServings: " + recipe.Servings;
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
