 using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    namespace Tiani.P_Bites_Bytes.Models.ViewModels
    {
        public class EditProductViewModel
        {
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

            [Display(Name = "New Image")]
            public HttpPostedFileBase ImageFile { get; set; }

        //    [Display(Name = "Category")]
        //    public int SelectedCategoryId { get; set; }
      


        //public List<CategoryViewModel> Categories { get; set; }
        }
    }
