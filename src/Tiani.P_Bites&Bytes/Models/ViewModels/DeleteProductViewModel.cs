using System;

namespace Tiani.P_Bites_Bytes.Models.ViewModels
{
    public class DeleteProductViewModel
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int StockLevel { get; set; }
        public string ImageUrl { get; set; }
        public DateTime StockUpdatedOn { get; set; }
        public string CategoryName { get; set; }
    }
}

