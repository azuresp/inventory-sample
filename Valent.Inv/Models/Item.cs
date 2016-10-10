using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Valent.Inv.Models
{
    public class Item
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("expires")]
        public DateTime Expires { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
