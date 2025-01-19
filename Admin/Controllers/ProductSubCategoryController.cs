using BusinessLogic.IServices;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Product;
using System.Linq;

namespace Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductSubCategoryController : Controller
    {
        private readonly IProductSubCategoryService _ProductSubCategoryService;
        private readonly IProductCategoryService _ProductCategoryService;

        // Constructor
        public ProductSubCategoryController(IProductSubCategoryService ProductSubCategoryService, IProductCategoryService ProductCategoryService)
        {
            _ProductSubCategoryService = ProductSubCategoryService;
            _ProductCategoryService = ProductCategoryService;
        }

        // Index Action
        public async Task<IActionResult> Index(ProductSubCategoryModel model)
        {
            var categories = await _ProductSubCategoryService.GetSubCategoriesAsync();

            return View(categories);
        }

        // Add Category - GET
        [HttpGet]
        public IActionResult AddSubCategory()
        {

            ViewBag.ProductCategories = ProductCategorySelectList();
            return View("AddEditSubCategory",new ProductSubCategoryModel());
        }

        // Add Category - POST
        [HttpPost]
        public IActionResult AddSubCategory(ProductSubCategoryModel model)
        {
            if (ModelState.IsValid)
            {
                _ProductSubCategoryService.AddSubCategory(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }






        //// Delete Category - POST
        //[HttpPost]
        //public IActionResult Delete(string name)
        //{
        //    if (_ProductSubCategoryService.HasRelatedProduct(name))
        //    {
        //        TempData["Error"] = "Cannot delete category with related products.";
        //        return RedirectToAction("Index");
        //    }

        //    var result = _ProductSubCategoryService.DeleteCategory(name);

        //    if (!result)
        //    {
        //        TempData["Error"] = "Category not found or could not be deleted.";
        //    }
        //    else
        //    {
        //        TempData["Success"] = "Category deleted successfully.";
        //    }

        //    return RedirectToAction("Index");
        //}


        // Edit Category - GET
        [HttpGet]
        public IActionResult EditSubCategory(int id)
        {
            var category = _ProductSubCategoryService.GetSubCategoryById(id);
            if (category == null)
            {
                TempData["Error"] = "Category not found.";
                return RedirectToAction("Index");
            }
            ViewBag.ProductCategories = ProductCategorySelectList();
            return View("AddEditSubCategory", category);
        }

        // Edit Category - POST
        [HttpPost]
        public IActionResult EditSubCategory(ProductSubCategoryModel model)
        {
            if (ModelState.IsValid)
            {
                _ProductSubCategoryService.UpdateSubCategory(model);
                TempData["Success"] = "Category updated successfully.";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index", "ProductSubCategory");
        }


        private List<SelectListItem> ProductCategorySelectList()
        {
            return _ProductCategoryService.GetCategory().ProductCategoryList.Select(category => new SelectListItem
            {
                Value = category.ProductCategoryId.ToString(),
                Text = category.Name
            }).ToList();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteSubCategory(int subcategoryId)
        {
            var isDeleted = _ProductSubCategoryService.DeleteSubCategory(subcategoryId);

            if (!isDeleted)
            {
                TempData["Error"] = "Failed to delete the subcategory. It may not exist.";
                return RedirectToAction("Index");
            }

            TempData["Success"] = "Subcategory deleted successfully!";
            return RedirectToAction("Index");
        }




    }
}
