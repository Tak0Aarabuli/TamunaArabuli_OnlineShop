using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.IServices;
using DataAccess.EF;
using Microsoft.EntityFrameworkCore;
using Models.Product;

namespace BusinessLogic.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly EFContext _context;

        public ProductCategoryService(EFContext context)
        {
            _context = context;
        }

        // Get all categories for listing
        public ProductCategoryModel GetCategory()
        {
            ProductCategoryModel model = new ProductCategoryModel();
            model.ProductCategoryList = _context.ProductCategories
                .Select(c => new ProductCategoryListItem
                {
                    Name = c.Name,
                    ProductCategoryId=c.ProductCategoryId,
                    ProductCount = c.ProductSubcategories.SelectMany(s => s.Products).Count(),
                }).ToList();

            return model;
        }

        // Add a new category
        public void AddCategory(CreateProductCategoryViewModel model)
        {
            var category = new ProductCategory
            {
                Name = model.Name,
                Rowguid = Guid.NewGuid(),
                ModifiedDate = DateTime.Now,
            };

            _context.ProductCategories.Add(category);
            _context.SaveChanges();
        }

        // Delete a category if there are no related products
        public bool DeleteCategory(string categoryName)
        {
            var category = _context.ProductCategories
                .Include(c => c.ProductSubcategories)
                .FirstOrDefault(c => c.Name == categoryName);

            if (category == null || category.ProductSubcategories.Any())
            {
                return false; // Cannot delete if products are related
            }

            _context.ProductCategories.Remove(category);
            _context.SaveChanges();
            return true;
        }

        // Check if the category has any related products
        public bool HasRelatedProduct(string categoryName)
        {
            var category = _context.ProductCategories
                .Include(c => c.ProductSubcategories)
                .ThenInclude(s => s.Products)
                .FirstOrDefault(c => c.Name == categoryName);

            return category != null && category.ProductSubcategories
                .SelectMany(s => s.Products)
                .Any();
        }



        // Get category by Id for editing
        public CreateProductCategoryViewModel GetCategoryById(int id)
        {
            var category = _context.ProductCategories.FirstOrDefault(c => c.ProductCategoryId == id);
            if (category == null) return null;

            return new CreateProductCategoryViewModel
            {
                Id = category.ProductCategoryId,
                Name = category.Name
            };
        }

        // Update category details
        public void UpdateCategory(CreateProductCategoryViewModel model)
        {
            var category = _context.ProductCategories.FirstOrDefault(c => c.ProductCategoryId == model.Id);

            if (category != null)
            {
                category.Name = model.Name;
                category.ModifiedDate = DateTime.Now;

                _context.ProductCategories.Update(category);
                _context.SaveChanges();
            }

        }
    }
}
