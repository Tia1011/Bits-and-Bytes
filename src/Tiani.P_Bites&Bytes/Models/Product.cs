using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Tiani.P_Bites_Bytes.Models
{
    public class Product
    {
        [Key]
        public string ProductId {  get; set; }

        [Display(Name ="Product Name")]
        public string ProductName { get; set; }
       
        public string Description { get; set; }


        [Display(Name = "Unit Price")]
        public double Price { get; set; }


        [Display(Name = "Units in Stock")]
        public int StockLevel { get; set; }


        [Display(Name = "Image")]
        public string ImageUrl { get; set; }


        [Display(Name = "Stock Last Updates")]
        public DateTime StockUpdatedOn { get; set; }

        [NotMapped]
        public string Status {  get; set; } //used for low stock

        //updates when new stock arrives
        public void ReStock(int quantity)
        {
            StockLevel = StockLevel + quantity;
        }
        //updates when product is sold
        public void UpdateStock(int quantity)
        {
            StockLevel -= quantity;

        }


        //navigational property
        public Product() { OrderLines = new List<OrderLine>(); }
        public virtual ICollection<OrderLine> OrderLines { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}