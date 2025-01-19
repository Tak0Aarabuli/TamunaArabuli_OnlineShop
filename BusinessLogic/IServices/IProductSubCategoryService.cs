
using DataAccess.EF;
using Models.Product;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.IServices
{
    public interface IProductSubCategoryService
    {
        Task<IEnumerable<ProductSubCategoryModel>> GetSubCategoriesAsync();

        void AddSubCategory(ProductSubCategoryModel model);

        bool DeleteSubCategory(int subCategoryId);

        void UpdateSubCategory(ProductSubCategoryModel model);
        ProductSubCategoryModel GetSubCategoryById(int id);

    }

}
