using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Product
{
    public class ProductViewModel
    {
        public List<ProductListItemModel> ProductList { get; set; } = new List<ProductListItemModel>();
        public int TotalRecords { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        // public ICollection<SalesOrderDetail> SalesOrderDetails { get; set; } = new List<SalesOrderDetail>();
    }
    public class ProductDetailModel : ProductListItemModel
    {

        public string? Size { get; set; }
    }

    public class ProductListItemModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string ProductNumber { get; set; } = null!;
        public string? ProductColor { get; set; }
        public decimal ListPrice { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int OrderCount { get; set; }
       

    }

    //public class CreateProductViewModel
    //{
    //    public int ProductId { get; set; }
    //    public string ProductName { get; set; }= null!; 
    //    public string ProductNumber { get; set; }=null!;
    //    public string? ProductColor { get; set; }
    //    public short SafetyStockLevel { get; set; }
    //    public short ReorderPoint { get; set; }
    //    public decimal StandardCost { get; set; }
    //    public decimal ListPrice { get; set; }
    //    public string? Size { get; set; }
        
    //    public int DaysToManufacture { get; set; }

    //    //ProductSubcategoryID and ModelID
    //    public DateTime SellStartDate { get; set; }

        
    //    public int OrderCount { get; set; }
        
    //    public int ProductSubCategoryId { get; set; }   
    //    public int ProductModelId { get; set; } 
    //    public List<ProductSubCategoryListItem>? ProductSubCategory { get; set; } 
    //    public List<ProductModelListItem>? ProductModel { get; set; } 

    //}
    public class CreateProductViewModel
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product Name is required.")]
        [StringLength(50, ErrorMessage = "Product Name cannot exceed 50 characters.")]
        public string ProductName { get; set; } = null!;

        [Required(ErrorMessage = "Product Number is required.")]
        [StringLength(25, ErrorMessage = "Product Number cannot exceed 25 characters.")]
        public string ProductNumber { get; set; } = null!;

        [Required(ErrorMessage = "Make Flag is required.")]
        public bool MakeFlag { get; set; }

        [Required(ErrorMessage = "Finished Goods Flag is required.")]
        public bool FinishedGoodsFlag { get; set; }

        [StringLength(15, ErrorMessage = "Color cannot exceed 15 characters.")]
        public string? ProductColor { get; set; }

        [Required(ErrorMessage = "Safety Stock Level is required.")]
        [Range(1, short.MaxValue, ErrorMessage = "Safety Stock Level must be greater than 0.")]
        public short SafetyStockLevel { get; set; }

        [Required(ErrorMessage = "Reorder Point is required.")]
        [Range(1, short.MaxValue, ErrorMessage = "Reorder Point must be greater than 0.")]
        public short ReorderPoint { get; set; }

        [Required(ErrorMessage = "Standard Cost is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Standard Cost must be greater than 0.")]
        public decimal StandardCost { get; set; }

        [Required(ErrorMessage = "List Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "List Price must be greater than 0.")]
        public decimal ListPrice { get; set; }

        [StringLength(5, ErrorMessage = "Size cannot exceed 5 characters.")]
        public string? Size { get; set; }

        [StringLength(3, ErrorMessage = "Size Unit Measure Code cannot exceed 3 characters.")]
        public string? SizeUnitMeasureCode { get; set; }

        [StringLength(3, ErrorMessage = "Weight Unit Measure Code cannot exceed 3 characters.")]
        public string? WeightUnitMeasureCode { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Weight must be greater than 0 if provided.")]
        public decimal? Weight { get; set; }

        [Required(ErrorMessage = "Days to Manufacture is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Days to Manufacture must be 0 or greater.")]
        public int DaysToManufacture { get; set; }

        [StringLength(2, ErrorMessage = "Product Line cannot exceed 2 characters.")]
        public string? ProductLine { get; set; }

        [StringLength(2, ErrorMessage = "Class cannot exceed 2 characters.")]
        public string? Class { get; set; }

        [StringLength(2, ErrorMessage = "Style cannot exceed 2 characters.")]
        public string? Style { get; set; }

        public int? ProductSubCategoryId { get; set; }

        public int? ProductModelId { get; set; }

        [Required(ErrorMessage = "Sell Start Date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Sell Start Date must be a valid date.")]
        public DateTime SellStartDate { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Sell End Date must be a valid date.")]
        public DateTime? SellEndDate { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Discontinued Date must be a valid date.")]
        public DateTime? DiscontinuedDate { get; set; }

        public Guid RowGuid { get; set; } = Guid.NewGuid();

        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        public int OrderCount { get; set; }
        public List<ProductSubCategoryListItem>? ProductSubCategory { get; set; }
        public List<ProductModelListItem>? ProductModel { get; set; }
    }
    public class ProductSubCategoryListItem
    {
        public int Id { get; set; } 
        public string Name { get; set; } = null!;
        
    }
    public class ProductModelListItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

    }
}
