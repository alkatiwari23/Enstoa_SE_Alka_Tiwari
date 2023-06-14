using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task.Model
{
    public class GetRecordResponseModel
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public int QuantityInStock { get; set; }
    }
}
