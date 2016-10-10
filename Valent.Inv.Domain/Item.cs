using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valent.Inv.Domain
{
    public class Item
    {
        public string Label { get; set; }
        public DateTime Expires { get; set; }
        public string Type { get; set; }

        //This is kind of trivial, but thought I'd use this as a chance to discuss my
        //preference for non-anemic domains.
        public bool IsExpired => Expires < DateTime.UtcNow;
    }
}
