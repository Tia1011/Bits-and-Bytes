using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tiani.P_Bites_Bytes.Models
{
    public class SpecialOfferCode
    {
        [Key]
        public string OfferId { get; set; }

        public string OfferCode { get; set; }

        public bool OfferUsed { get; set; }

        public DateTime ValidityDate { get; set; }

        public bool OfferInValid { get; set; }

        public int PercentOff { get; set; }

        public string CustomerEmail { get; set; }

        // Change to ICollection<string> for multiple customers
        public ICollection<string> CustomerIds { get; set; }

        // Change navigation property to ICollection<Customer>
        public virtual ICollection<Customer> Customers { get; set; }
    }
}
