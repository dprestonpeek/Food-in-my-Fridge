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
    public partial class SuggestedRecipes : ContentPage
    {
        List<Recipe> recipes;
        public SuggestedRecipes(List<Recipe> recipes)
        {
            InitializeComponent();
            this.recipes = recipes;

            LabelStack.Children.Clear();
            foreach (Recipe rec in recipes)
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
    }
}
