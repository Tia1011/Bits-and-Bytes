using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace Tiani.P_Bites_Bytes.Models
{
    public class BitsAndBytesDbContext : IdentityDbContext<User>
    {

        //db sets of category, posts and comments
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<PaymentCard> PaymentCard { get; set; }
        public DbSet<SpecialOfferCode> SpecialOfferCodes { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Customer> Customers { get; set; }


        //create dbcontext
        public BitsAndBytesDbContext()
            : base("BitsAndBytesConnection", throwIfV1Schema: false)
            {
                Database.SetInitializer(new DatabaseInitialiser());
            }

            public static BitsAndBytesDbContext Create()
            {
                return new BitsAndBytesDbContext();
            }



        

    }
}