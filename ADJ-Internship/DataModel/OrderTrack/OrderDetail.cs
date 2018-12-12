using ADJ.DataModel.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ADJ.DataModel.OrderTrack
{
    public class OrderDetail : EntityBase
    {
        //unknown
        public string ItemNumber { get; set; }

        public string Line { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(30)]
        public string Warehouse { get; set; }

        [StringLength(30)]
        public string Colour { get; set; }

        [StringLength(30)]
        public string Size { get; set; }

        //unknown
        public string Item { get; set; }

        [Required]
        //[Display(Name = "Item Quantity")]
        public float Quantity { get; set; }

        public float ReviseQuantity { get; set; }

        [Required]
        public float Cartons { get; set; }

        [Required]
        public float Cube { get; set; }

        [Required]
        public float KGS { get; set; }

        [Required]
        //[Display(Name = "Unit Price")]
        public float UnitPrice { get; set; }

        //Item Quantity*Unit Price = Total Price
        public float TotalPrice
        {
            get
            {
                return Quantity * UnitPrice;
            }
        }

        [Required]
        //[Display(Name = "Retail Price")]
        public float RetailPrice { get; set; }

        //Item Quantity*Retail Price = Total Retail Price
        public float TotalRetailPrice
        {
            get
            {
                return Quantity * RetailPrice;
            }
        }

        //[RegularExpression("[^0-9]", ErrorMessage = "Tariff must be numeric")]
        public string Tariff { get; set; }

        [Required]
        //[ForeignKey("OrderModel")]
        public int OrderId { get; set; }

        //public virtual Order Order { get; set; }
    }
}
