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
	public partial class Loading : ContentPage
    {
        public bool IsLoading { get; set; }
        public bool success = false;

        public enum LoadType { LogginIn, SavingInventory, SearchingRecipes}
        LoadType loadType;
        public Object Result;

        public Loading(LoadType theLoadType)
        {
            InitializeComponent();
            loadType = theLoadType;
            ChangeLoadingScreen();
        }

        public void ChangeLoadingScreen()
        {
            switch(loadType)
            {
                case LoadType.LogginIn:
                    loadText.Text = "\n\n\n\nChecking your credentials...";
                    break;
                case LoadType.SavingInventory:
                    loadText.Text = "\n\n\n\nSaving your inventory...";
                    break;
                case LoadType.SearchingRecipes:
                    loadText.Text = "\n\n\n\nSearching Recipes...";
                    break;
            }
        }
	}
}