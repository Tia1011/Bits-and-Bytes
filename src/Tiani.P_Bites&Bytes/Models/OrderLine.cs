using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Tiani.P_Bites_Bytes.Models
{
    public class OrderLine
    {
        [Key]
        public int OrderLineId {  get; set; }
        public int Quantity { get; set; }

        public double Price { get; set; }
        [Display(Name ="Amount")]
        public double LineTotal { get; set;}



        //navigation property
        [ForeignKey("Product")]
        public string ProductId { get; set; }
        public Product Product { get; set; }

  
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}