using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tiani.P_Bites_Bytes.Models.ViewModels
{
    public class UpdateStockViewModel
    {
        public string ProductId { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        public int StockLevel { get; set; }
    }
}