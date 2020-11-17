using MobileApplication.Models;
using MobileApplication.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileApplication.ViewModels
{
    public class RecipesViewModel : BaseViewModel
    {
        public Command LoadItemsCommand { get; set; }
        public List<Recipe> Recipes { get; set; }

        public RecipesViewModel(List<Recipe> recipes)
        {
            Title = "Recipe Search";
            Recipes = new List<Recipe>();

            //Task.Run(async() => await ExecuteLoadItemsCommand());
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            var listView = new ListView
            {
                ItemTemplate = new DataTemplate(typeof(RecipeItemCell))
            };

            foreach (Recipe recipe in recipes)
            {
                RecipeStore.AddItemAsync(recipe);
                Recipes.Add(new Recipe { Label = recipe.Label, Source = recipe.Source, Image = recipe.Image });
            }
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                //Make sure items are clear, then assign Recipe Items in list to search results
                //Recipes.Clear();
                await Task.Delay(500);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }

    internal class RecipeItemCell : ViewCell
    {
        // To register the LongTap/Tap-and-hold gestures once the item model has been assigned
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            RegisterGestures();
        }

        private void RegisterGestures()
        {
            var deleteOption = new MenuItem()
            {
                Text = "Delete",
                IconImageSource = "deleteIcon.png", //Android uses this, for example
                CommandParameter = ((Recipe)BindingContext).Label
            };
            deleteOption.Clicked += DeleteOption_Clicked;
            ContextActions.Add(deleteOption);

            //Repeat for the other 4 options

        }

        void DeleteOption_Clicked(object sender, EventArgs e)
        {
            //To retrieve the parameters (if is more than one, you should use an object, which could be the same ItemModel 
            int idToDelete = (int)((MenuItem)sender).CommandParameter;
            //your delete actions
        }
        //Write the eventHandlers for the other 4 options
    }
}