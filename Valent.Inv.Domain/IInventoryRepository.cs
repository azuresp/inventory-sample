using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valent.Inv.Domain
{
    public interface IInventoryRepository
    {
        void Set(Item item);
        Item Get(string label);
        bool DeleteIfExists(string label);
        IReadOnlyCollection<Item> GetExpired();
    }
}
