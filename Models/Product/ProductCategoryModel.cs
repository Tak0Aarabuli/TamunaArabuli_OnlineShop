using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Models.Product
{
    public class ProductCategoryModel
    {

        public List<ProductCategoryListItem> ProductCategoryList { get; set; }


    }
    public class ProductCategoryListItem
    {
        public string Name { get; set; }
        public int ProductCount { get; set; }
        public int ProductCategoryId { get; set; }
        
    }

    public class CreateProductCategoryViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Category Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }
    }

    
}
