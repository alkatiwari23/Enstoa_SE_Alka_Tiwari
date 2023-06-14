using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Task.Model
{
    [Table("ShoppingCart")]
    public class ShoppingCart
    {
#nullable enable
        public string? Itemid { get; set; }
        public string? Quantity { get; set; }
    }
}
