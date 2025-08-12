using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tiani.P_Bites_Bytes.Models.ViewModels
{
    public class ChangeRoleViewModel
    {
        public string Username { get; set; }
        public string OldRole { get; set; }
        public ICollection<SelectListItem> Roles { get; set; }

        [Required, Display(Name = "Role")]
        public string Role { get; set; }
    }
}