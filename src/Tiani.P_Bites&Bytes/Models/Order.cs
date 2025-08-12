using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Tiani.P_Bites_Bytes.Models
{
    public class Order
    {

        [Display(Name = "Order Id")]
        public int OrderId { get; set; }

        [Display(Name = "Date")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Total")]
        public double OrderTotal { get; set; }
        public double DiscountValue { get; set; }
        [Display(Name = "Paid")]
        public bool IsPaid { get; set; }
        public OrderStatus OrderStatus { get; set; }


        //navigational property
        public Payment Payments { get; set; }

        [ForeignKey("Customer")]
        public string EmailAddress { get; set; }
        public Customer Customer { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        public Order()
        {
            OrderLines = new List<OrderLine>() ;
        }

        public virtual ICollection<OrderLine> OrderLines { get; set; }

       
    }

    public enum OrderStatus { Started,Cancelled,Completed,}
}