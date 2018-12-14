using ADJ.DataModel.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ADJ.DataModel.OrderTrack
{
    public class OrderDetail : EntityBase
    {
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

        public string Item { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        public float ReviseQuantity { get; set; }

        [Required]
        public float Cartons { get; set; }

        [Required]
        public float Cube { get; set; }

        [Required]
        public float KGS { get; set; }

        [Required]
        public float UnitPrice { get; set; }

        public float TotalPrice
        {
            get
            {
                return (float)Quantity * UnitPrice;
            }
        }

        [Required]
        public float RetailPrice { get; set; }

        public float TotalRetailPrice
        {
            get
            {
                return (float)Quantity * RetailPrice;
            }
        }

        public string Tariff { get; set; }

        [Required]
        public int OrderId { get; set; }

        public virtual Order Order { get; set; }
    }
}
