using Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.IServices
{
    public interface IProductService
    {
        ProductViewModel GetProducts(ProductViewModel model);
        bool AddProduct (CreateProductViewModel model, out string msg);
       
        IEnumerable<ProductSubCategoryListItem> GetProductSubCategories();
        IEnumerable<ProductModelListItem> GetProductModels();
        bool DeleteProduct(int productId);
        bool UpdateProduct(CreateProductViewModel model, out string msg);
        CreateProductViewModel GetProductById(int id);

    }
}
