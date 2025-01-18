using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Product
{
    public class ProductViewModel
    {
        public List<ProductListItemModel> ProductList { get; set; } = new List<ProductListItemModel>();
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

    public class CreateProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }= null!; 
        public string ProductNumber { get; set; }=null!;
        public string? ProductColor { get; set; }
        public short SafetyStockLevel { get; set; }
        public short ReorderPoint { get; set; }
        public decimal StandardCost { get; set; }
        public decimal ListPrice { get; set; }
        public string? Size { get; set; }
        
        public int DaysToManufacture { get; set; }

        //ProductSubcategoryID and ModelID
        public DateTime SellStartDate { get; set; }

        
        public int OrderCount { get; set; }
        
        public int ProductSubCategoryId { get; set; }   
        public int ProductModelId { get; set; } 
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
