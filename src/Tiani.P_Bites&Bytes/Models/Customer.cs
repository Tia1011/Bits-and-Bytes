using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Tiani.P_Bites_Bytes.Models
{
    public class Customer
    {
        //Display Attributes

        [Key]

        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Display(Name = "First Name")]
        public string Firstname { get; set; }

        [Display(Name = "Last Name")]
        public string Lastname { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        [Display(Name = "Post Code")]
        public string Postcode { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        public DateTime DateRegistered { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string OTP { get; set; }

        //navigation property
        // [ForeignKey("DiscountStatus")]
        //public string DiscountStatusId { get; set; }
        //public DiscountStatus DiscountStatus { get; set; }

        public string DiscountStatusId { get; set; }
       
        public List<Payment> Payments { get; set; }

       public Customer() : base() 
        {
            Orders = new List<Order>();
            PaymentCards = new List<PaymentCard>();
        }
        public virtual ICollection<PaymentCard> PaymentCards { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}