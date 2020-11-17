using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MobileApplication.Models;

namespace MobileApplication.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        List<Item> inventoryItems;

        public MockDataStore()
        {
            inventoryItems = new List<Item>();
            var mockItems = new List<Item>
            {
                new Item { UPC = Guid.NewGuid().ToString(), ProductName = "First item", Description="This is an item description." },
                new Item { UPC = Guid.NewGuid().ToString(), ProductName = "Second item", Description="This is an item description." },
                new Item { UPC = Guid.NewGuid().ToString(), ProductName = "Third item", Description="This is an item description." },
                new Item { UPC = Guid.NewGuid().ToString(), ProductName = "Fourth item", Description="This is an item description." },
                new Item { UPC = Guid.NewGuid().ToString(), ProductName = "Fifth item", Description="This is an item description." },
                new Item { UPC = Guid.NewGuid().ToString(), ProductName = "Sixth item", Description="This is an item description." },
            };

            foreach (var item in mockItems)
            {
                inventoryItems.Add(item);
            }
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            inventoryItems.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = inventoryItems.Where((Item arg) => arg.UPC == item.UPC).FirstOrDefault();
            inventoryItems.Remove(oldItem);
            inventoryItems.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = inventoryItems.Where((Item arg) => arg.UPC == id).FirstOrDefault();
            inventoryItems.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(inventoryItems.FirstOrDefault(s => s.UPC == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(inventoryItems);
        }
    }

    public class MockRecipeStore : IDataStore<Recipe>
    {
        List<Recipe> recipeItems;

        public MockRecipeStore()
        {
            recipeItems = new List<Recipe>();
            var mockItems = new List<Recipe>
            {
                new Recipe { Label = "RecipeName", Source = "Recipe source" },
                new Recipe { Label = "RecipeName", Source = "Recipe source" },
                new Recipe { Label = "RecipeName", Source = "Recipe source" },
                new Recipe { Label = "RecipeName", Source = "Recipe source" },
                new Recipe { Label = "RecipeName", Source = "Recipe source" },
            };

            foreach (var item in mockItems)
            {
                recipeItems.Add(item);
            }
        }

        public async Task<bool> AddItemAsync(Recipe recipe)
        {
            recipeItems.Add(recipe);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Recipe recipe)
        {
            var oldItem = recipeItems.Where((Recipe arg) => arg.Label == recipe.Label).FirstOrDefault();
            recipeItems.Remove(oldItem);
            recipeItems.Add(recipe);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = recipeItems.Where((Recipe arg) => arg.Label == id).FirstOrDefault();
            recipeItems.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Recipe> GetItemAsync(string id)
        {
            return await Task.FromResult(recipeItems.FirstOrDefault(s => s.Label == id));
        }

        public async Task<IEnumerable<Recipe>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(recipeItems);
        }

        Task<Recipe> IDataStore<Recipe>.GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Recipe>> IDataStore<Recipe>.GetItemsAsync(bool forceRefresh)
        {
            throw new NotImplementedException();
        }
    }
}