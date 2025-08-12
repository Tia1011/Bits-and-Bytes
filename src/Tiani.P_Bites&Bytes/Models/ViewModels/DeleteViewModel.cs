using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tiani.P_Bites_Bytes.Models.ViewModels
{
    public class DeleteViewModel
    {
        [Display(Name = "First Name")]
        public string Firstname { get; set; }

        [Display(Name = "Last Name")]
        public string Lastname { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }

       
        [DataType(DataType.PostalCode)]

        [Display(Name = "Post Code")]
        public string Postcode { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }


        [Display(Name = "Date Hired")]
        public DateTime DateHired { get; set; }



        [Display(Name = "Role")]
        public string Role { get; set; }
        // public Role SelectedRole { get; set; }
        public ICollection<SelectListItem> Roles { get; set; }


    }
}