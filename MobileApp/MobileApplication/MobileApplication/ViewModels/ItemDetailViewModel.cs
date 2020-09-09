using System;

using MobileApplication.Models;

namespace MobileApplication.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Item Item { get; set; }
        public ItemDetailViewModel(Item item = null)
        {
            Title = item?.ProductName;
            Item = item;
        }
    }
}
