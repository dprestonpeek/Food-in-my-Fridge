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

        public enum LoadType { LogginIn, SavingInventory}
        LoadType loadType;
        public bool success = false;
        string username = "";
        string password = "";

        public Loading(LoadType theLoadType)
        {
            InitializeComponent();
            loadType = theLoadType;
        }

        public Loading(LoadType theLoadType, string username, string password)
		{
            InitializeComponent ();
            loadText.Text = "Checking your credentials...";
            loadType = theLoadType;
            this.username = username;
            this.password = password;
        }

        string DetermineLoadFunction()
        {
            switch(loadType)
            {
                case LoadType.LogginIn:
                    break;
                case LoadType.SavingInventory:
                    return "Saving your inventory...";
            }
            return "Loading...";
        }
	}
}