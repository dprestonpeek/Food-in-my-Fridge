using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MobileApplication.Models;

namespace MobileApplication.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        List<Item> items;

        public MockDataStore()
        {
            items = new List<Item>();
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
                items.Add(item);
            }
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = items.Where((Item arg) => arg.UPC == item.UPC).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Item arg) => arg.UPC == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.UPC == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}