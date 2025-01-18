using BusinessLogic.IServices;
using DataAccess.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Models.Product;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class ProductService : IProductService
    {
        private readonly EFContext _context;


        public ProductService(EFContext context)
        {
            _context = context;
        }

        // Get Products for listing
        public ProductViewModel GetProducts(ProductViewModel model)
        {
            model.ProductList = _context.Products
                .Select(p => new ProductListItemModel
                {
                    ProductId = p.ProductId,
                    ProductName = p.Name,
                    ProductNumber = p.ProductNumber,
                    ProductColor = p.Color,
                    ListPrice = p.ListPrice,
                    ModifiedDate = p.ModifiedDate,
                    OrderCount = p.PurchaseOrderDetails.Sum(x => x.OrderQty),
                }).ToList();

            return model;
        }



        public IEnumerable<ProductSubCategoryListItem> GetProductSubCategories()
        {
            return _context.ProductSubcategories
                .Select(sub => new ProductSubCategoryListItem
                {
                    Id = sub.ProductSubcategoryId,
                    Name = sub.Name
                })
                .ToList();
        }

        public IEnumerable<ProductModelListItem> GetProductModels()
        {
            return _context.ProductModels
                .Select(mod => new ProductModelListItem
                {
                    Id = mod.ProductModelId,
                    Name = mod.Name
                })
                .ToList();
        }






        // Add a new Product
        public bool AddProduct(CreateProductViewModel model, out string msg)
        {
            try
            {


                // Validate required fields
                if (model == null) throw new ArgumentNullException(nameof(model));
                if (string.IsNullOrWhiteSpace(model.ProductName) || string.IsNullOrWhiteSpace(model.ProductNumber))
                {
                    msg = "Product name and product number are required.";
                    return false;
                }


                // Check for unique ProductNumber
                if (_context.Products.Any(p => p.ProductNumber == model.ProductNumber))
                {

                    msg = "The ProductNumber must be unique.";
                    return false;
                }


                var product = new Product
                {
                    Name = model.ProductName,
                    ProductNumber = model.ProductNumber,
                    MakeFlag = true, // Defaulted to true as an example, update as needed
                    FinishedGoodsFlag = true, // Defaulted to true as an example, update as needed
                    Color = model.ProductColor,
                    SafetyStockLevel = model.SafetyStockLevel,
                    ReorderPoint = model.ReorderPoint,
                    StandardCost = model.StandardCost,
                    ListPrice = model.ListPrice,
                    Size = model.Size,
                    
                    DaysToManufacture = model.DaysToManufacture,
                    SellStartDate = model.SellStartDate,
                    Rowguid = Guid.NewGuid(),
                    ModifiedDate = DateTime.Now

                };

                // Add the product to the database
                _context.Products.Add(product);
                _context.SaveChanges();
                msg = "Product added successfully.";
                return true;


            }
            //catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("unique") == true)
            //{
            //    Console.WriteLine("A database error occurred: The ProductNumber must be unique.");
            //    throw new ArgumentException("The ProductNumber must be unique.", ex);
            //}
            //catch (ArgumentException ex)
            //{
            //    Console.WriteLine($"Validation error: {ex.Message}");
            //    throw;
            //}
            catch (Exception ex)
            {
                msg = "An unexpected error occurred while adding the product: {ex.Message}";
                return false;
            }
        }



        //delete
        public bool DeleteProduct(int productId)
        {


            try
            {
                // Fetch the product along with related PurchaseOrderDetails to minimize database queries
                var product = _context.Products
                    .Include(p => p.PurchaseOrderDetails)
                    .FirstOrDefault(p => p.ProductId == productId);

                // Check if the product exists
                if (product == null)
                {
                    return false; // Product not found
                }

                // Check if the product is related to any orders
                if (product.PurchaseOrderDetails.Any())
                {
                    return false; // Product cannot be deleted as it is related to orders
                }

                // Remove the product
                _context.Products.Remove(product);
                _context.SaveChanges();

                return true; // Deletion successful
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting product: {ex.Message}");
                return false; // Indicate failure
            }

        }






        // Get category by Id for editing
        public CreateProductViewModel GetProductById(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if (product == null) return new();

            return new CreateProductViewModel
            {
                ProductId = product.ProductId,
                ProductNumber = product.ProductNumber,
                ProductName = product.Name,
                ProductColor = product.Color,
                ReorderPoint = product.ReorderPoint,
                StandardCost = product.StandardCost,
                SafetyStockLevel = product.SafetyStockLevel,
                ListPrice = product.ListPrice,
                Size = product.Size ?? string.Empty, // Handle null for Size
               
                SellStartDate = product.SellStartDate,
                DaysToManufacture = product.DaysToManufacture,
                


            };
        }




        // Update product details
        public bool UpdateProduct(CreateProductViewModel model, out string msg)
        {
            try
            {
                var product = _context.Products.FirstOrDefault(c => c.ProductId == model.ProductId);

                if (product != null) 
                {
                    
                    product.ProductNumber = model.ProductNumber;
                    product.Name = model.ProductName;
                    product.Color = model.ProductColor;
                    product.ReorderPoint = model.ReorderPoint;
                    product.StandardCost = model.StandardCost;
                    product.SafetyStockLevel = model.SafetyStockLevel;
                    product.ListPrice = model.ListPrice;
                    product.Size = model.Size;
                    
                    product.SellStartDate = model.SellStartDate;
                    product.DaysToManufacture = model.DaysToManufacture;
                    product.ModifiedDate = DateTime.Now;



                    _context.SaveChanges();
                    msg = "Product edited successfully.";
                    return true;

                }
                msg = " Error editing product";
                return false;
            }

            catch (Exception ex)
            {
               msg = ex.InnerException?.Message ?? ex.Message;
                return false; 
            }
        }


    }

}


