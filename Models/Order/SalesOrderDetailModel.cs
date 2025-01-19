using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.Order
{
    public class SalesOrderDetailModel
    {
        [Required]
        public int SalesOrderId { get; set; }

        [Required]
        public int SalesOrderDetailId { get; set; }

        [StringLength(25, ErrorMessage = "Carrier Tracking Number cannot exceed 25 characters.")]
        public string? CarrierTrackingNumber { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Order Quantity must be greater than 0.")]
        public short OrderQty { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int SpecialOfferId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit Price must be greater than 0.")]
        [DataType(DataType.Currency)]
        public decimal UnitPrice { get; set; }

        [Required]
        [Range(0, 1, ErrorMessage = "Unit Price Discount must be between 0 and 1.")]
        public decimal UnitPriceDiscount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Line Total cannot be negative.")]
        public decimal LineTotal { get; set; } // Computed field; no need for input.

        [Required]
        public Guid Rowguid { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ModifiedDate { get; set; }

        public string val { get; set; }
    }


    public class ProductDropdownItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class SpecialOfferProductDropdownItem
    {
        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
