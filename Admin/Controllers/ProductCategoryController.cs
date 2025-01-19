using BusinessLogic.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Product;
using System.Linq;

namespace Admin.Controllers
{
    public class ProductCategoryController : Controller
    {
        private readonly IProductCategoryService _productCategoryService;

        // Constructor
        public ProductCategoryController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        // Index Action
        public IActionResult Index(ProductCategoryModel model)
        {
            var categories = _productCategoryService.GetCategory();
            return View(categories);
        }

        // Add Category - GET
        [HttpGet]
        public IActionResult AddCategory()
        {
            return View(new CreateProductCategoryViewModel());
        }

        // Add Category - POST
        [HttpPost]
        public IActionResult AddCategory(CreateProductCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                _productCategoryService.AddCategory(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }





        
        // Delete Category - POST
        [HttpPost]
        public IActionResult Delete(string name)
        {
            if (_productCategoryService.HasRelatedProduct(name))
            {
                TempData["Error"] = "Cannot delete category with related products.";
                return RedirectToAction("Index");
            }

            var result = _productCategoryService.DeleteCategory(name);

            if (!result)
            {
                TempData["Error"] = "Category not found or could not be deleted.";
            }
            else
            {
                TempData["Success"] = "Category deleted successfully.";
            }

            return RedirectToAction("Index");
        }


        // Edit Category - GET
        [HttpGet]
        public IActionResult EditCategory(int id)
        {
            var category = _productCategoryService.GetCategoryById(id);
            if (category == null)
            {
                TempData["Error"] = "Category not found.";
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // Edit Category - POST
        [HttpPost]
        public IActionResult EditCategory(CreateProductCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                _productCategoryService.UpdateCategory(model);
                TempData["Success"] = "Category updated successfully.";
                return RedirectToAction("Index");
            }

            return View(model);
        }





    }
}
