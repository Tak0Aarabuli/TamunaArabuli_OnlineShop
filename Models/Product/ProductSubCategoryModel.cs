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
        public int ProductSubcategoryId { get; set; }
        public int ProductCategoryId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime ModifiedDate { get; set; }

    }

    public class ProductSubCategoryCreateModel
    {

        [Required(ErrorMessage = "Product category is required.")]
        public int ProductCategoryId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot be more than 50 characters.")]
        public string Name { get; set; } = null!;
    }

    
}
