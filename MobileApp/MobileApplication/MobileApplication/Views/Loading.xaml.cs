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

        public enum LoadType { CheckingCredentials, LoggingIn, CreatingAccount, SavingInventory, SearchingRecipes}
        LoadType loadType;
        public Object Result;

        public Loading(LoadType theLoadType)
        {
            InitializeComponent();
            loadType = theLoadType;
            ChangeLoadingScreen(theLoadType);
        }

        public void ChangeLoadingScreen(LoadType theLoadType)
        {
            switch(theLoadType)
            {
                case LoadType.CheckingCredentials:
                    loadText.Text = "\n\n\n\nChecking your credentials...";
                    break;
                case LoadType.LoggingIn:
                    loadText.Text = "\n\n\n\nLogging you in...";
                    break;
                case LoadType.CreatingAccount:
                    loadText.Text = "\n\n\n\nCreating your account...";
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