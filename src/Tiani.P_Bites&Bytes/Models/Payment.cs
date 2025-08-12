using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Tiani.P_Bites_Bytes.Models
{
    public class Payment
    {
        [Key,ForeignKey("Order")]
        public int PaymentId { get; set; }
        public string PaymentType { get; set; }
        public int PaymentStatus { get; set; }
        public double PaymentAmount {  get; set; }

        //navigation property

        
       
        public Order Order { get; set; }

        [ForeignKey("Customer")]
        public string EmailAddress { get; set; }
        public Customer Customer { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("PaymentCard")]
        public int CardId { get; set; }
        public PaymentCard PaymentCard { get; set; }
    }
}