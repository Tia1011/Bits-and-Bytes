using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tiani.P_Bites_Bytes.Models.ViewModels
{
    public class CreateStaffViewModel
    {
        ////attributes used and displayed when creating staff

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
        public string Email { get; set; }

        [Display(Name = "Email Confirmed")]
        public bool EmailConfirm { get; set; }

        [Required, Display(Name = "Password")]
        public string Password { get; set; }

        public double Salary {  get; set; }

        //public string DateHired { get; set; }
        
        [Required(ErrorMessage = "DateHired must be entered")]
        [Display(Name = "Date Hired")]
        [DataType(DataType.Date)]
        public string DateHired { get; set; }






        [Display(Name = "Role")]
        public string Role { get; set; }


        public IEnumerable<SelectListItem> Roles { get; set; }

    }
}