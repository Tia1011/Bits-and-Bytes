using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tiani.P_Bites_Bytes.Models.ViewModels
{
    public class CreateProductViewModel
    {
        [Key]
        public string ProductId { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Unit Price")]
        public double Price { get; set; }

        [Required]
        [Display(Name = "Units in Stock")]
        public int StockLevel { get; set; }

       
        [Display(Name = "Image")]
        public string ImageUrl { get; set; }


        [Display(Name = "Stock Last Updates")]
        public DateTime StockUpdatedOn { get; set; }

        [Display(Name = "Category")]
        public int SelectedCategoryId { get; set; }  // Property to store the selected category ID

        public List<CategoryViewModel> Categories { get; set; } // List of available categories
    }

    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }



}