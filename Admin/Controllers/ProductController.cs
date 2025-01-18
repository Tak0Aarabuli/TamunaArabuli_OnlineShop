using BusinessLogic.IServices;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Product;

namespace Admin.Controllers
{
    [Authorize (Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        // Constructor
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult Index(ProductViewModel model)
        {
            var products = _productService.GetProducts(model);
            return View(products);
        }


        // Add Product - GET
        [HttpGet]
        public IActionResult AddProduct()
        {
            var model = new CreateProductViewModel
            {
                ProductSubCategory = _productService.GetProductSubCategories()
                    .Select(sub => new ProductSubCategoryListItem
                    {
                        Id = sub.Id,
                        Name = sub.Name
                    })
                    .ToList(),
                ProductModel = _productService.GetProductModels()
                    .Select(mod => new ProductModelListItem
                    {
                        Id = mod.Id,
                        Name = mod.Name
                    })
                    .ToList()
            };

            return View(model);
        }

        // Add Product - POST
        [HttpPost]
        public IActionResult AddProduct(CreateProductViewModel model)
        {
            var msg = string.Empty;
            if (ModelState.IsValid)
            {
               var result = _productService.AddProduct(model, out msg);
                if (result)
                {
                    return RedirectToAction(nameof (Index));
                } 
            }

            // Reload dropdowns if model validation fails
            model.ProductSubCategory = _productService.GetProductSubCategories()
                .Select(sub => new ProductSubCategoryListItem
                {
                    Id = sub.Id,
                    Name = sub.Name
                })
                .ToList();
            model.ProductModel = _productService.GetProductModels()
                .Select(mod => new ProductModelListItem
                {
                    Id = mod.Id,
                    Name = mod.Name
                })
                .ToList();

            return View(model);
        }


        //[HttpPost]
        //public IActionResult DeleteProduct(int productId)
        //{


        //    try
        //    {
        //        //if (productId <= 0)
        //        //{
        //        //    TempData["Error"] = "Invalid product ID.";
        //        //    return RedirectToAction("Index");
        //        //}

        //        var isDeleted = _productService.DeleteProduct(productId);
        //        if (!isDeleted)
        //        {
        //            TempData["Error"] = "Product cannot be deleted because it is related to orders.";
        //        }
        //        else
        //        {
        //            TempData["Success"] = "Product deleted successfully.";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Error"] = $"An error occurred: {ex.Message}";
        //    }

        //    return RedirectToAction("Index");
        //}



        [HttpPost]
        public IActionResult DeleteProduct(int productId)
        {
            var result = _productService.DeleteProduct(productId);
            if (result)
            {
                TempData["Message"] = "Product deleted successfully.";
            }
            else
            {
                TempData["Error"] = "Cannot delete the product. It may be linked to existing orders or does not exist.";
            }

            return RedirectToAction("Index");
        }






        // Edit Product - GET
        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                TempData["Error"] = "Product not found.";
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // Edit Category - POST
        [HttpPost]
        public IActionResult EditProduct(CreateProductViewModel model)
        {
            var msg = string.Empty;
            if (ModelState.IsValid)
            {

                var result = _productService.UpdateProduct(model, out msg);
                if (result)
                {
                    TempData["Success"] = msg;
                    return RedirectToAction("Index");
                }
                TempData["Error"] = msg;
            }

            return View(model);
        }






    }
}
