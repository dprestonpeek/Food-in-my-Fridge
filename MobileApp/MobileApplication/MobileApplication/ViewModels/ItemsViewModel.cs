using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using MobileApplication.Models;
using MobileApplication.Views;

namespace MobileApplication.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<Item> Items { get; set; }
        public ObservableCollection<string> DBItems { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel()
        {
            Title = "My Fridge";
            Items = new ObservableCollection<Item>();
            DBItems = new ObservableCollection<string>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            var listView = new ListView();
            listView.ItemTemplate = new DataTemplate(typeof(InventoryItemCell));

            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Item;
                Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            Database db = new Database();
            string[,] inventory = db.GetUserInventory(App.Username);

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                for (int i = 0; i < inventory.Length; i++)
                {
                    Items.Add(new Item { UPC = inventory[i, 0], ProductName = inventory[i,1], Description = inventory[i,2], ImageUrl = inventory[i,3], Quantity = inventory[i, 4]});
                }
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

    internal class InventoryItemCell : ViewCell
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
                Icon = "deleteIcon.png", //Android uses this, for example
                CommandParameter = ((Item)BindingContext).UPC
            };
            deleteOption.Clicked += deleteOption_Clicked;
            ContextActions.Add(deleteOption);

            //Repeat for the other 4 options

        }
        void deleteOption_Clicked(object sender, EventArgs e)
        {
            //To retrieve the parameters (if is more than one, you should use an object, which could be the same ItemModel 
            int idToDelete = (int)((MenuItem)sender).CommandParameter;
            //your delete actions
        }
        //Write the eventHandlers for the other 4 options
    }
}