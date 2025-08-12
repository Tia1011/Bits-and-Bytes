using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Tiani.P_Bites_Bytes.Models.ViewModels
{
    public class EditStaffViewModel
    {


        [Required]
        [Display(Name = "First Name")]
        public string Firstname { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string Lastname { get; set; }

        [Required]
        public string Address1 { get; set; }
        public string Address2 { get; set; }



        [Required]



        [Display(Name = "Post Code")]
        public string Postcode { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "Email Confirmed")]
        public bool EmailConfirm { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }


        [Required(ErrorMessage = "DateHired must be entered")]
        [Display(Name = "Date Hired")]
        [DataType(DataType.Date)]
        public string DateHired { get; set; }




        [Display(Name = "Role")]
        public IEnumerable<SelectListItem> Roles { get; set; }
        public string Role { get; set; }

    }
}