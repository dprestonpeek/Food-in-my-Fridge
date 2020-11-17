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
		}
	}
}