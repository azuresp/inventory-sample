using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Valent.Inv.Models
{
    public class ItemPutRequest
    {
        /// <summary>
        /// The date and time when the item will expire.  Required.
        /// </summary>
        [Required]
        [JsonProperty("expires")]
        public DateTime? Expires { get; set; }

        [Required]
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
