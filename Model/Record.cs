using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Task.Model
{
    [Table("Record")]
    public class Record
    {
        [Key]
        public string ItemId { get; set; }
#nullable enable
        public string? ItemName { get; set; }
        public string? AvailableStocks { get; set; }
    }
}
