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
    public class ProductSubCategoryService : IProductSubCategoryService
    {
        private readonly EFContext _context;

        public ProductSubCategoryService(EFContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductSubCategoryModel>> GetSubCategoriesAsync()
        {
            var query = from subcategory in _context.ProductSubcategories
                        join category in _context.ProductCategories
                            on subcategory.ProductCategoryId equals category.ProductCategoryId
                        join product in _context.Products
                            on subcategory.ProductSubcategoryId equals product.ProductSubcategoryId into productGroup
                        select new ProductSubCategoryModel
                        {
                            ProductCategoryId = subcategory.ProductCategoryId,
                            ProductCategoryName = category.Name,
                            ProductSubCategoryId = subcategory.ProductSubcategoryId,
                            ProductSubCategoryName = subcategory.Name,
                            ProductsCount = productGroup.Count(),
                            ModifiedDate = subcategory.ModifiedDate
                        };

            return await query.ToListAsync(); 
        }


        public void AddSubCategory(ProductSubCategoryModel model)
        {
            var subCategory = new ProductSubcategory
            {
                Name = model.ProductSubCategoryName,
                ProductCategoryId= model.ProductCategoryId,
                Rowguid = Guid.NewGuid(),
                ModifiedDate = DateTime.Now,
            };

            _context.ProductSubcategories.Add(subCategory);
            _context.SaveChanges();
        }

        public bool DeleteSubCategory(int id)
        {
            
            var subCategory = _context.ProductSubcategories.FirstOrDefault(sc => sc.ProductSubcategoryId == id);
            if (subCategory == null)
            {
                return false; 
            }

            _context.ProductSubcategories.Remove(subCategory);
            _context.SaveChanges();

            return true; 
        }




        public ProductSubCategoryModel GetSubCategoryById(int id)
        {
            var subcategory = _context.ProductSubcategories.FirstOrDefault(c => c.ProductSubcategoryId == id);
            if (subcategory == null) return null;

            return  new ProductSubCategoryModel
            {
                ProductCategoryId = subcategory.ProductCategoryId,
                ProductSubCategoryId = subcategory.ProductSubcategoryId,
                ProductSubCategoryName = subcategory.Name,
            };

        }

        //// Update category details
        public void UpdateSubCategory(ProductSubCategoryModel model)
        {
            var subcategory = _context.ProductSubcategories.FirstOrDefault(c => c.ProductSubcategoryId == model.ProductSubCategoryId);

            if (subcategory != null)
            {
                subcategory.Name = model.ProductSubCategoryName;
                subcategory.ProductCategoryId = model.ProductCategoryId;
                subcategory.ModifiedDate = DateTime.Now;

                _context.ProductSubcategories.Update(subcategory);
                _context.SaveChanges();
            }

        }
    }
}
