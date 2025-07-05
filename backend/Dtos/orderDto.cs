using backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Dtos
{
    public class orderDto
    {
        public int ordId { get; set; }
        public int? ordItemId { get; set; }
        public Item Item { get; set; }
        public int? itemId { get; set; }
        public string itemName { get; set; }
        public int? Quantity { get; set; }
        public Category ctg { get; set; }

        //public int CategoryId { get; set; }
        //public string CategoryName { get; set; }
    }
}
