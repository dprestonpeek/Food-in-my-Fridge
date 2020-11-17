using MobileApplication.ViewModels;
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
	public partial class RecipeList : ContentPage
	{
        private RecipesViewModel viewModel;

        public RecipeList(List<Recipe> recipes, RecipesViewModel viewModel)
		{
			InitializeComponent();

            //give the XAML page context so it can view recipe search results
            BindingContext = this.viewModel = viewModel;
        }

        //When a recipe is selected
        async void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            if (RecipesListView.SelectedItem != null)
            {
                Recipe theRecipe = (Recipe)RecipesListView.SelectedItem;
                await DisplayAlert("Recipe", "You selected " + theRecipe.Label, "OK");
            }

            //Deselect Item
            RecipesListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Recipes.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}