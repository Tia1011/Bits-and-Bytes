using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System;
using System.Security.Permissions;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace Tiani.P_Bites_Bytes.Models
{
    public class User : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }



       

        [Display(Name = "First Name")]
        public string Firstname { get; set; }

        [Display(Name = "Last Name")]
        public string Lastname { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        [Display(Name = "Post Code")]
        public string Postcode { get; set; }

        //[Display(Name = "Phone Number")]
        //public string PhoneNumber { get; set; }

        public double Salary { get; set; }


        [Display(Name = "DateHired")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM//dd/yyyy}")]
        public DateTime DateHired { get; set; }


        public List<Order> Orders { get; set; }
        public List<Payment> Payments { get; set; }


        //needing the ApplicationUserManager to get the users current role
        private ApplicationUserManager userManager;



        [NotMapped]
        public string StaffRole
        {
            get
            {
                if (userManager == null)
                {
                    //initialize  userManager
                    userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                }
                return userManager.GetRoles(Id).Single();
            }
        }

    }

    
}