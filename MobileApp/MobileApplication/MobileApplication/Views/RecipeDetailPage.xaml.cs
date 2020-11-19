using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApplication.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RecipeDetailPage : ContentPage
	{
        Recipe recipe;

		public RecipeDetailPage(Recipe recipe)
		{
			InitializeComponent ();
            this.recipe = recipe;
            RecipeLabel.Text = recipe.Label;
            RecipeImage.Source = recipe.Image;
            RecipeSource.Text = "Retrieved from: " + recipe.Source;
            RecipeCalories.Text = "Calories: " + recipe.Calories;
            RecipeServings.Text = "Servings: " + recipe.Servings;
            RecipeTime.Text = "Time: " + recipe.Time;
        }

        private void ShowIngredients_Clicked(object sender, EventArgs e)
        {
            LabelStack.Children.Clear();
            foreach (Ingredient ing in recipe.Ingredients)
            {
                Button button = new Button { Text = ing.text, ImageSource = ing.image };
                LabelStack.Children.Add(button);
                button.Clicked += ShowIngredientOptions;
            }
        }

        private async void ShowIngredientOptions(object sender, EventArgs e)
        {
            Ingredient thisIng = null;
            Button button = (Button)sender;
            string buttonText = button.Text;

            foreach (Ingredient ing in recipe.Ingredients)
            {
                string ingredientLabel = ing.text;
                if (ingredientLabel == buttonText)
                {
                    thisIng = ing;
                }
            }

            if (await DisplayAlert(thisIng.text, "Add this item to your inventory?", "Yes", "No"))
            {
                await Navigation.PushModalAsync(new NewItemPage(thisIng));
            }
        }
    }
}