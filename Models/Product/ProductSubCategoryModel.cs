using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Models.Product
{

    public class ProductSubCategoryModel
    {
        public int ProductSubCategoryId { get; set; }
        public int ProductCategoryId { get; set; }
        public string? ProductCategoryName { get; set; }
        public string ProductSubCategoryName { get; set; } = null!;
        public int ProductsCount { get; set; }
        public DateTime ModifiedDate { get; set; }


    }

    public class CreateProductSubCategoryModel
    {
        public int? ProductSubCategoryId { get; set; }

        [Required(ErrorMessage = "Product category is required.")]
        public int ProductCategoryId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot be more than 50 characters.")]
        public string Name { get; set; } = null!;
    }

    
}
