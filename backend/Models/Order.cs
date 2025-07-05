using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{

    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        // public List<Item> Items { get; set; }
        // public int Quantity { get; set; }
        public List<OrderItem> orditms { get; set; } 


    }

    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        // public string Name { get; set; }

        public int Quantity { get; set; }


        // Foreign key to Order
        // public int OrderId { get; set; }
        public Order? Order { get; set; }

        // Foreign key to Item
        //public int ItemId { get; set; }
        public Item? Item { get; set; }



        // Navigation back to parent

        // Uncomment if you want to include category in OrderItem
        // public int CategoryId { get; set; }
        // public  Category Ctg{ get; set; }  
        // public Order Order { get; set; }
        // public int OrderId { get; set; }

    }
}
