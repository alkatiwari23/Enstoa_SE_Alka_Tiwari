using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Task.Model
{
    
    public class ShoppingCartViewModel
    {
        public string Itemid { get; set; }
        public int Quantity { get; set; }
        public string ItemName { get; set; }
    }
}
