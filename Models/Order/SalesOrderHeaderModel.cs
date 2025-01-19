using Models.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.Order
{
    public class SalesOrderHeaderModel
    {
        [Required]
        public int SalesOrderId { get; set; }

        [Required]
        [Range(1, byte.MaxValue, ErrorMessage = "Revision number must be greater than 0.")]
        public byte RevisionNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Ship Date")]
        public DateTime? ShipDate { get; set; }

        [Required]
        [Range(0, byte.MaxValue, ErrorMessage = "Status must be between 0 and 255.")]
        public byte Status { get; set; }

        [Required]
        [Display(Name = "Online Order")]
        public bool OnlineOrderFlag { get; set; }

        [StringLength(25, ErrorMessage = "Purchase Order Number cannot exceed 25 characters.")]
        public string? PurchaseOrderNumber { get; set; }

        [StringLength(15, ErrorMessage = "Account Number cannot exceed 15 characters.")]
        public string? AccountNumber { get; set; }

        [Required]
        [Display(Name = "Customer ID")]
        public int CustomerId { get; set; }

        [Display(Name = "Customer Name")]
        public string? CustomerName { get; set; }

        [Required]
        [Display(Name = "Bill To Address")]
        public int BillToAddressId { get; set; }

        [Required]
        [Display(Name = "Ship To Address")]
        public int ShipToAddressId { get; set; }

        [Required]
        [Display(Name = "Ship Method")]
        public int ShipMethodId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Subtotal must be greater than 0.")]
        public decimal SubTotal { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Tax Amount cannot be negative.")]
        public decimal TaxAmt { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Freight cannot be negative.")]
        public decimal Freight { get; set; }

        [StringLength(128, ErrorMessage = "Comment cannot exceed 128 characters.")]
        public string? Comment { get; set; }

        [Display(Name = "Total Due")]
        public decimal TotalDue => SubTotal + TaxAmt + Freight;

        public List<SalesOrderDetailModel>? SalesOrderDetails { get; set; }
    }


    public class AddressDropdownItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class ShipMethodDropdownItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
