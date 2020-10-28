using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApplication.Models
{
    public enum MenuItemType
    {
        Inventory,
        About,
        NewItem,
        ExamplePage,
        RecipeSearch,
        Logout
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
