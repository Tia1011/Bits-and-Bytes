using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using System.Collections;
using static Tiani.P_Bites_Bytes.Models.User;
using System.Web.UI.WebControls;
using Stripe;

namespace Tiani.P_Bites_Bytes.Models
{
    public class DatabaseInitialiser : DropCreateDatabaseAlways<BitsAndBytesDbContext>
    {
        protected override void Seed(BitsAndBytesDbContext context)
        {
            //base.Seed(context);


            //if there are no records stored in the users table
            if (!context.Users.Any())
            {
                //first we are going to create some roles and store them in the roles table
                //to create and store roles we need a roles table
                RoleManager<IdentityRole> identityRoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                //if manager doesnt exist
                if (!identityRoleManager.RoleExists("Manager"))
                {
                    //then we create one
                    identityRoleManager.Create(new IdentityRole("Manager"));
                }

                //if SalesAssistant doesnt exist
                if (!identityRoleManager.RoleExists("SalesAssistant"))
                {
                    //then we create one
                    identityRoleManager.Create(new IdentityRole("SalesAssistant"));
                }



                //save the new roles to the database
                context.SaveChanges();


                //********************CREATE USERS**********************

                //to create users-customers or employees we need a Usermanager
                UserManager<User> UserManager = new UserManager<User>(new UserStore<User>(context));




                //Super literal password validation for  password for seeds
                UserManager.PasswordValidator = new PasswordValidator
                {
                    RequireDigit = false,
                    RequiredLength = 1,
                    RequireLowercase = false,
                    RequireNonLetterOrDigit = false,
                    RequireUppercase = false,
                };

                //CREATE A manager 
                var manager1 = new User
                {
                    UserName = "Alvin@bitsnbytes.com",
                    Email = "Alvin@bitsnbytes.com",
                    Firstname = "Alvin",
                    Lastname = "Grove",
                    Address1 = "10/A",
                    Address2 = "Poker Street,Glasgow",
                    Postcode = "G0 0RF",
                    EmailConfirmed = true,
                    Salary = 21,
                    DateHired = DateTime.Now.AddYears(-6),
                    PhoneNumber = "0100000000",


                };


                //FIRST check if the manager exists in the database
                if (UserManager.FindByName("Alvin@bitsnbytes.com") == null)
                {
                    //add admin to the users table
                    UserManager.Create(manager1, "Alvin123");
                    //assign it to the manager role
                    UserManager.AddToRole(manager1.Id, "Manager");

                }

                //CREATE A manager 
                var manager2 = new User
                {
                    UserName = "tianiperera@gmail.com",
                    Email = "tianiperera@gmail.com",
                    Firstname = "Tiani",
                    Lastname = "Perera",
                    Address1 = "10/A",
                    Address2 = "Poker Street,Glasgow",
                    Postcode = "G0 0RF",
                    EmailConfirmed = true,
                    Salary = 23,
                    DateHired = DateTime.Now.AddYears(-6),


                };


                //FIRST check if the manager exists in the database
                if (UserManager.FindByName("tianiperera@gmail.com") == null)
                {
                    //add admin to the users table
                    UserManager.Create(manager2, "Tiani123");
                    //assign it to the manager role
                    UserManager.AddToRole(manager2.Id, "Manager");

                }


                //create a  sales assistant
                var SalesAssistant1 = new User
                {
                    UserName = "sarah@bitsnbytes.com",
                    Email = "sarah@bitsnbytes.com",
                    Firstname = "Sarah",
                    Lastname = "Bernard",
                    Address1 = "10",
                    Address2 = "Happy Street,Glasgow",
                    Postcode = "G0 5ST",
                    EmailConfirmed = true,
                    Salary = 10.65,
                    DateHired = DateTime.Now.AddYears(-5),


                };


                //CREATE a SalesAssistant
                //FIRST check if the SalesAssistant exists in the database
                if (UserManager.FindByName("sarah@bitsnbytes.com") == null)
                {
                    //add SalesAssistant to the users table
                    UserManager.Create(SalesAssistant1, "Sarah123");
                    //assign it to the SalesAssistant role
                    UserManager.AddToRole(SalesAssistant1.Id, "SalesAssistant");

                }

                //create a  sales assistant
                var SalesAssistant2 = new User
                {
                    UserName = "alex@bitsnbytes.com",
                    Email = "alex@bitsnbytes.com",
                    Firstname = "Alex",
                    Lastname = "Cole",
                    Address1 = "120A",
                    Address2 = "Townhead Street,Glasgow",
                    Postcode = "G2 5AT",
                    EmailConfirmed = true,
                    Salary = 9.45,
                    DateHired = DateTime.Now.AddYears(-1),


                };


                //CREATE a SalesAssistant
                //FIRST check if the SalesAssistant exists in the database
                if (UserManager.FindByName("alex@bitsnbytes.com") == null)
                {
                    //add SalesAssistant to the users table
                    UserManager.Create(SalesAssistant2, "Alex123");
                    //assign it to the SalesAssistant role
                    UserManager.AddToRole(SalesAssistant2.Id, "SalesAssistant");

                }



                //create a  sales assistant
                var SalesAssistant3 = new User
                {
                    UserName = "amelia@bitsnbytes.com",
                    Email = "amelia@bitsnbytes.com",
                    Firstname = "Amelia",
                    Lastname = "Rose",
                    Address1 = "101",
                    Address2 = "Hooded Street,Glasgow",
                    Postcode = "G2 8B4",
                    EmailConfirmed = true,
                    Salary = 9.45,
                    DateHired = DateTime.Now.AddYears(-2),


                };


                //CREATE a SalesAssistant
                //FIRST check if the SalesAssistant exists in the database
                if (UserManager.FindByName("amelia@bitsnbytes.com") == null)
                {
                    //add SalesAssistant to the users table
                    UserManager.Create(SalesAssistant3, "Amelia123");
                    //assign it to the SalesAssistant role
                    UserManager.AddToRole(SalesAssistant3.Id, "SalesAssistant");

                }


                context.SaveChanges();


                //create a customer
                var customer1 = new Customer
                {
                    EmailAddress = "sashenperera@gmail.com",
                    Firstname = "Sashen",
                    Lastname = "Perera",
                    Address1 = "101",
                    Address2 = "Bran Street,Glasgow",
                    Postcode = "G4 2B3",
                    PhoneNumber = "0718512511",
                    DateRegistered = new DateTime(2024, 04, 03),
                    DateOfBirth = new DateTime(2002, 11, 05),
                    DiscountStatusId = ""
                };

                context.Customers.Add(customer1);

                //create a customer
                var customer2 = new Customer
                {
                    EmailAddress = "sonie@yahoo.com",
                    Firstname = "Sonie",
                    Lastname = "Graham",
                    Address1 = "55",
                    Address2 = "Heathrow Street,Hamilton",
                    Postcode = "ML4 2B3",
                    PhoneNumber = "0716900977",
                    DateRegistered = new DateTime(2024, 04, 01),
                    DateOfBirth = new DateTime(1998, 10, 25),
                    DiscountStatusId = "5OFF"
                };

                context.Customers.Add(customer2);

                //create a customer
                var customer3 = new Customer
                {
                    EmailAddress = "john.doe@example.com",
                    Firstname = "John",
                    Lastname = "Doe",
                    Address1 = "123",
                    Address2 = "Maple Street",
                    Postcode = "ABC123",
                    PhoneNumber = "0745551234",
                    DateRegistered = new DateTime(2024, 03, 30),
                    DateOfBirth = new DateTime(1995, 10, 15),
                    DiscountStatusId = "10OFF"
                };

                context.Customers.Add(customer3);

                //create a customer
                var customer4 = new Customer
                {
                    EmailAddress = "jane.smith@example.com",
                    Firstname = "Jane",
                    Lastname = "Smith",
                    Address1 = "789",
                    Address2 = "Oak Avenue",
                    Postcode = "XYZ789",
                    PhoneNumber = "0745555678",
                    DateRegistered = new DateTime(2024, 02, 10),
                    DateOfBirth = new DateTime(2000, 01, 01),
                    DiscountStatusId = "VATFREE"
                };

                context.Customers.Add(customer4);

                //create a customer
                var customer5 = new Customer
                {
                    EmailAddress = "tiani@perera.me",
                    Firstname = "ffff",
                    Lastname = "dddd",
                    Address1 = "789",
                    Address2 = "Oak Avenue",
                    Postcode = "XYZ789",
                    PhoneNumber = "0745555678",
                    DateRegistered = new DateTime(2024, 02, 10),
                    DateOfBirth = new DateTime(2000, 01, 01),
                    DiscountStatusId = "VATFREE"
                };

                context.Customers.Add(customer5);


                context.SaveChanges();

                //CREATE CATEGORIES
                var computerParts = new Category {CategoryId=1, Name = "Computer Parts" };
                var computerSystems = new Category {CategoryId=2, Name = "Computer Systems" };

                context.Categories.Add(computerParts);
                context.Categories.Add(computerSystems);
                context.SaveChanges();

                // Create a special offer code
                var specialOfferCode = new SpecialOfferCode
                {
                    OfferId="1234",
                    OfferCode = "15off",
                    PercentOff = 15,
                    ValidityDate = DateTime.Now.AddDays(1),
                    OfferInValid = false,
                    CustomerEmail = "sashenperera@gmail.com",
                    OfferUsed = false
                };

                context.SpecialOfferCodes.Add(specialOfferCode);

                context.SaveChanges();

                //CREATE PRODUCTS
                // Computer Parts
                context.Products.Add(new Product()
                {
                    ProductId = "CP001",
                    ProductName = "Intel Core i7-10700K Processor",
                    Category = computerParts,
                    StockUpdatedOn = DateTime.Now.AddMonths(-3),
                    Description = "10th Gen, 8 Cores, 16 Threads, 3.80 GHz",
                    Price = 349.99,
                    StockLevel = 50,
                    ImageUrl = "/Images/Products/Intel Core i7-10700k Processor.jpg"
                });


                context.Products.Add(new Product()
                {
                    ProductId = "CP002",
                    ProductName = "NVIDIA GeForce RTX 3080 Graphics Card",
                    Category = computerParts,
                    StockUpdatedOn = DateTime.Now.AddMonths(-1),
                    Description = "10GB GDDR6X, PCIe 4.0, RGB Lighting",
                    Price = 699.99,
                    StockLevel = 0,
                    ImageUrl = "/Images/Products/GraphicsCards NVIDIA.jpg",
                });



                context.Products.Add(new Product()
                {
                    ProductId = "CP003",
                    ProductName = "Corsair Vengeance LPX 16GB DDR4 RAM",
                    Category = computerParts,
                    StockUpdatedOn = DateTime.Now.AddMonths(-2),
                    Description = "3200MHz, CL16, Black",
                    Price = 89.99,
                    StockLevel = 20,
                    ImageUrl = "/Images/Products/Product3.jpg"
                });


                context.Products.Add(new Product()
                {
                    ProductId = "CP004",
                    ProductName = "Samsung 970 EVO Plus 1TB NVMe SSD",
                    Category = computerParts,
                    StockUpdatedOn = DateTime.Now.AddDays(-10),
                    Description = "PCIe Gen 3.0, M.2 (2280), V-NAND",
                    Price = 169.99,
                    StockLevel = 15,
                    ImageUrl = "/Images/Products/Product4.jpg"
                });

                context.Products.Add(new Product()
                {
                    ProductId = "CP005",
                    ProductName = "Cooler Master Hyper 212 RGB CPU Cooler",
                    Category = computerParts,
                    StockUpdatedOn = DateTime.Now.AddYears(-1),
                    Description = "120mm PWM Fan, RGB Lighting",
                    Price = 49.99,
                    StockLevel = 25,
                    ImageUrl = "/Images/Products/Product5.jpg"
                });

                context.Products.Add(new Product()
                {
                    ProductId = "CP006",
                    ProductName = "ASUS ROG Strix X570-E Gaming Motherboard",
                    Category = computerParts,
                    StockUpdatedOn= DateTime.Now.AddYears(-2),
                    Description = "ATX, AM4, PCIe 4.0, DDR4, Wi-Fi 6",
                    Price = 299.99,
                    StockLevel = 10,
                    ImageUrl = "/Images/Products/Product6.jpg"
                });

                // Computer Systems
                context.Products.Add(new Product()
                {
                    ProductId = "CS001",
                    ProductName = "Gaming PC - Ryzen Edition",
                    Category = computerSystems,
                    StockUpdatedOn = DateTime.Now.AddDays(-1),
                    Description = "AMD Ryzen 9 5900X, NVIDIA RTX 3080, 32GB RAM, 1TB SSD + 2TB HDD",
                    Price = 2799.99,
                    StockLevel = 10,
                    ImageUrl = "/Images/Products/Product7.jpg"
                });

                context.Products.Add(new Product()
                {
                    ProductId = "CS002",
                    ProductName = "Workstation PC - Intel Edition",
                    Category = computerSystems,
                    StockUpdatedOn = DateTime.Now.AddYears(-2),
                    Description = "Intel Core i9-10900K, NVIDIA Quadro RTX 4000, 64GB RAM, 1TB NVMe SSD",
                    Price = 3599.99,
                    StockLevel = 5,
                    ImageUrl = "/Images/Products/Product8.jpg"
                });
                context.Products.Add(new Product()
                {
                    ProductId = "CS003",
                    ProductName = "Intel Core i5-10400",
                    Category = computerSystems,
                    StockUpdatedOn = DateTime.Now.AddYears(-2),
                    Description = "Intel Core i5-10400, 16GB RAM, 512GB SSD, Intel UHD Graphics 630",
                    Price = 899.99,
                    StockLevel = 8,
                    ImageUrl = "/Images/Products/Product9.jpg"
                });

                context.Products.Add(new Product()
                {
                    ProductId = "CS004",
                    ProductName = "Mini-ITX Gaming PC",
                    Category = computerSystems,
                    StockUpdatedOn = DateTime.Now.AddYears(-1),
                    Description = "AMD Ryzen 5 5600X, NVIDIA RTX 3060, 16GB RAM, 1TB NVMe SSD",
                    Price = 1499.99,
                    StockLevel = 7,
                    ImageUrl = "/Images/Products/Product10.jpg"
                });

                //w
                context.Products.Add(new Product()
                {
                    ProductId = "CS005",
                    ProductName = "Content Creation PC",
                    Category = computerSystems,
                    StockUpdatedOn = DateTime.Now.AddYears(-2),
                    Description = "Intel Core i7-11700K, 32GB RAM, 1TB NVMe SSD, NVIDIA RTX 3070",
                    Price = 2499.99,
                    StockLevel = 6,
                    ImageUrl = "/Images/Products/Product11.jpg"
                });

                context.Products.Add(new Product()
                {
                    ProductId = "CS006",
                    ProductName = "Student Laptop",
                    Category = computerSystems,
                    StockUpdatedOn = DateTime.Now.AddYears(-2),
                    Description = "Apple MacBook Air M1, 8GB RAM, 256GB SSD",
                    Price = 999.99,
                    StockLevel = 10,
                    ImageUrl = "/Images/Products/Product12.jpg"
                });

                context.SaveChanges();


                ////seed card types
                //context.CardTypes.Add(new CardType()
                //{
                //    CardTypeName = "Mastercard"
                //}) ;

                //context.CardTypes.Add(new CardType()
                //{
                //    CardTypeName = "Visa"
                //});

                //context.CardTypes.Add(new CardType()
                //{
                //    CardTypeName = "Visa debit"
                //});

                //context.CardTypes.Add(new CardType()
                //{
                //    CardTypeName = "American Express"
                //});

                //context.SaveChanges();
            }

        }
    }
}