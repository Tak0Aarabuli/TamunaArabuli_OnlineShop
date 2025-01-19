
using DataAccess.EF;
using Models.Product;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.IServices
{
    public interface IProductCategoryService
    {
        ProductCategoryModel GetCategory();

        void AddCategory(CreateProductCategoryViewModel model);

        bool DeleteCategory(string categoryName);
        bool HasRelatedProduct(string categoryName);

        void UpdateCategory(CreateProductCategoryViewModel model);
        CreateProductCategoryViewModel GetCategoryById(int id);

    }

}
