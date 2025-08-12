using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Tiani.P_Bites_Bytes.Models
{
    public class PaymentCard 
    {
        [Key]
        public int CardId {  get; set; }
        public int CardNumber { get; set; }
        public int CvvNumber { get; set; }
        public string AccountName { get; set; }

        public int ExpiryMonth { get; set; }

        public int ExpiryYear { get; set; }

        //navigational props
        [ForeignKey("Customer")]
        public string UserId {  get; set; }
        public Customer Customer {  get; set; }
       
        public PaymentCard()
        {
            Payments= new List<Payment>();
        }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}