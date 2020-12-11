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
            string keyword = Keyword.Text; //get user input from xaml entry
            string[] antiKeywords;
            try
            {
                antiKeywords = AntiKeywords.Text.Split(',');   //get antikeywords and split them at the commas
            }
            catch
            {
                antiKeywords = new string[] { AntiKeywords.Text };
            }
            bool balanced = BalancedCheck.IsChecked;
            bool highProtein = HighProteinCheck.IsChecked;
            bool lowFat = LowFatCheck.IsChecked;
            bool lowCarb = LowCarbCheck.IsChecked;
            bool lowSodium = LowSodiumCheck.IsChecked;

            bool vegetarian = VegetarianCheck.IsChecked;
            bool vegan = VeganCheck.IsChecked;
            bool peanutFree = PeanutFreeCheck.IsChecked;
            bool treenutFree = TreeNutFreeCheck.IsChecked;
            bool alcoholFree = AlcoholFreeCheck.IsChecked;

            List<DietLabels> dietLabels = new List<DietLabels>();
            if (balanced)
            {
                dietLabels.Add(DietLabels.BALANCED);
            }
            if (highProtein)
            {
                dietLabels.Add(DietLabels.HIGHPROTEIN);
            }
            if (lowFat)
            {
                dietLabels.Add(DietLabels.LOWFAT);
            }
            if (lowCarb)
            {
                dietLabels.Add(DietLabels.LOWCARB);
            }
            if (lowSodium)
            {
                dietLabels.Add(DietLabels.LOWSODIUM);
            }

            List<HealthLabels> healthLabels = new List<HealthLabels>();
            if (vegetarian)
            {
                healthLabels.Add(HealthLabels.VEGETARIAN);
            }
            if (vegan)
            {
                healthLabels.Add(HealthLabels.VEGAN);
            }
            if (peanutFree)
            {
                healthLabels.Add(HealthLabels.PEANUTFREE);
            }
            if (treenutFree)
            {
                healthLabels.Add(HealthLabels.TREENUTFREE);
            }
            if (alcoholFree)
            {
                healthLabels.Add(HealthLabels.ALCOHOLFREE);
            }

            recipes = db.GetRecipes(keyword, antiKeywords, dietLabels, healthLabels);  //gets list of recipe results

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
