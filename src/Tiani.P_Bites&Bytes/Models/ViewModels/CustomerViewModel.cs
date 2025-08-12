using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tiani.P_Bites_Bytes.Models.ViewModels
{
    public class CustomerViewModel
    {
        [Required, Display(Name = "First Name")]
        public string Firstname { get; set; }

        [Required, Display(Name = "Last Name")]
        public string Lastname { get; set; }
        [Required]
        public string Address1 { get; set; }
        public string Address2 { get; set; }


        [DataType(DataType.PostalCode)]

        [Display(Name = "Post Code")]
        public string Postcode { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required, Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        public DateTime DateRegistered { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

       
        public string DiscountStatusId { get; set; }

    }
}