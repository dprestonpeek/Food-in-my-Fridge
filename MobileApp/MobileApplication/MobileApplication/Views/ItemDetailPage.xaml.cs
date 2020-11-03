using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MobileApplication.Models;
using MobileApplication.ViewModels;
using System.Threading.Tasks;

namespace MobileApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel viewModel;

        public ItemDetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();
            ButtonEditItem.Clicked += ButtonEditItem_Clicked;
            ButtonDeleteItem.Clicked += ButtonDeleteItem_Clicked;
            BindingContext = this.viewModel = viewModel;
        }

        public ItemDetailPage()
        {
            InitializeComponent();
            var item = new Item
            {
                ProductName = "Item 1",
                Description = "This is an item description.",
            };

            viewModel = new ItemDetailViewModel(item);
            BindingContext = viewModel;
        }

        private void ButtonEditItem_Clicked(object sender, EventArgs e)
        {
            App.editingItem = true;
            App.ScannedUPC = viewModel.Item.UPC;
            Navigation.PushModalAsync(new NewItemPage());
        }

        private async void ButtonDeleteItem_Clicked(object sender, EventArgs e)
        {
            Database db = new Database();
            bool delete = await DisplayAlert("Delete Item", "Are you sure you want to delete this item?", "Yes", "No");
            if (delete)
            {
                db.RemoveFromUserInventory(App.Username, viewModel.Item.UPC);
                Application.Current.MainPage = new MainPage();
            }
        }
    }
}