using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Tiani.P_Bites_Bytes.Models.ViewModels
{
    public class SpecialOfferCodeViewModel
    {
        [Required]
        public string OfferCode { get; set; }

        [Display(Name = "Validity Date")]
        [DataType(DataType.Date)]

        [Required]
        public DateTime ValidityDate { get; set; }

        public bool OfferInValid { get; set; }

        [Display(Name = "Percentage Off")]
        public int PercentOff { get; set; }


        [Display(Name = "Customers")]
        public List<SelectListItem> Customers { get; set; }


        [Required]
        // Add property to hold selected customer IDs
        [Display(Name = "Selected Customers")]
        public List<string> SelectedCustomers { get; set; }
        public string CustomerEmail { get; set; }
    }
}

