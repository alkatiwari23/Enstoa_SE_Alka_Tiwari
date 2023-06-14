using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task.Model
{
    public class AddNewRecordRequest
    {
        public string ItemName { get; set; }
        public int QuantityInStock { get; set; }
    }
}
