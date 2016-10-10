using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valent.Inv.Domain;

namespace Valent.Inv.Infrastructure
{
    public class InMemoryInventoryRepository : IInventoryRepository
    {
        private readonly ConcurrentDictionary<string, Item> _items = new ConcurrentDictionary<string, Item>(); //managing the lifetime via IOC registration.

        public void Set(Item item)
        {
            _items[item.Label] = item;
        }

        public Item Get(string label)
        {
            return _items.ContainsKey(label) ? _items[label] : null;
        }

        public bool DeleteIfExists(string label)
        {
            Item removedItem;
            _items.TryRemove(label, out removedItem);

            return (removedItem != null);
        }

        public IReadOnlyCollection<Item> GetExpired()
        {
            return _items.Values.Where(i => i.IsExpired).ToArray(); 
        }
    }
}
