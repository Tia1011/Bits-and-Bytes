using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Tiani.P_Bites_Bytes.Models;
using Tiani.P_Bites_Bytes.Models.ViewModels;

namespace Tiani.P_Bites_Bytes.Controllers
{
    public class CustomerController : Controller
    {
        private BitsAndBytesDbContext context = new BitsAndBytesDbContext();

        private readonly ApplicationUserManager _userManager;

        public CustomerController(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }
        public CustomerController()
        {

        }

        public ActionResult CustomerLogin()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CustomerLogin(CustomerViewModel model)
        {


            Customer customer = context.Customers.FirstOrDefault(c => c.EmailAddress == model.EmailAddress);


            if (customer != null)
            {
                Order anyPrevOrder = context.Orders.FirstOrDefault(c => c.OrderStatus == OrderStatus.Started && c.EmailAddress != null);
                if (anyPrevOrder != null)
                {
                    context.Orders.Remove(anyPrevOrder);
                    context.SaveChanges();
                }
                Order currentOrder = await CreateOrder(customer.EmailAddress);
                TempData["AlertMessage"] = "Customer has been found, Items can be placed in the basket now";
                return RedirectToAction("Products", "Shop", new { email = model.EmailAddress });//, order = currentOrder });//email = model.EmailAddress

                // Redirect staff to OTP generation page
                // return RedirectToAction("GenerateOTP", "OTP", new { email = email });
            }
            else
            {
                // If customer not found, display error message
                TempData["ErrorMessage"] = "Customer not found. Please try again.";
                return RedirectToAction("CustomerLogin", "Customer");
            }
        }

        public async Task<Order> CreateOrder(string email)
        {
            // Step 1: Retrieve customer information based on the provided identifier
            Customer customer = context.Customers.FirstOrDefault(c => c.EmailAddress == email);

            string userId = User.Identity.GetUserId();


            // Step 2: Create a new order object
            Order newOrder = new Order
            {
                OrderDate = DateTime.Now,
                EmailAddress = email, // Assign the customer ID to the order
                OrderStatus = OrderStatus.Started,
                OrderTotal = 0, // Initial order total
                UserId = userId, // Assuming you've removed the UserId property from the Order class
                DiscountValue = 0,
            };

            newOrder.Customer = customer;//?

            // Step 3: Save the order to the database
            context.Orders.Add(newOrder);
            context.SaveChanges();
            return newOrder;
        }

        
        
        public ActionResult CustomerDetails()
        {

            
           var orderWithEmail = context.Orders.FirstOrDefault(c => c.OrderStatus == OrderStatus.Started && c.EmailAddress != null);
           
           string emailAddress = orderWithEmail?.EmailAddress;

            Customer customer = context.Customers.FirstOrDefault(c=>c.EmailAddress == emailAddress);
            return View(customer);
        }
        public ActionResult CustomerLogOff()
        {
            var orderWithEmail = context.Orders.FirstOrDefault(c => c.OrderStatus == OrderStatus.Started && c.EmailAddress != null);
            orderWithEmail.OrderStatus = OrderStatus.Cancelled;
            context.SaveChanges();

            return RedirectToAction("CustomerLogin");
        }
        public ActionResult RegisterCustomer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterCustomer(Customer model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if the email already exists in the database
                    var existingCustomer = context.Customers.FirstOrDefault(c => c.EmailAddress == model.EmailAddress);
                    if (existingCustomer != null)
                    {
                        // Email already exists, return error message
                        Order newOrder = await CreateOrder(existingCustomer.EmailAddress);
                        TempData["ErrorMessage"] = "Customer already has an account";
                        return RedirectToAction("CustomerLogin");
                       // return RedirectToAction("Products", "Shop", new { email = model.EmailAddress });//, order = currentOrder });//email = model.EmailAddress

                      
                    }

                    // Set additional properties like DateRegistered if needed
                    model.DateRegistered = DateTime.Now;

                    // Save the new customer to the database
                    context.Customers.Add(model);
                    context.SaveChanges();

                    Order currentOrder = await CreateOrder(model.EmailAddress);
                    TempData["AlertMessage"] = "Customer has been Created, Items can be placed in the basket now";
                    return RedirectToAction("Products", "Shop", new { email = model.EmailAddress });

                }
                catch (Exception ex)
                {
                    // Handle exceptions appropriately
                    ModelState.AddModelError("", "Registration failed. Please try again later.");
                    return View("RegisterCustomer", model);
                }
            }

            // If ModelState is not valid, return to registration page
            return View("CustomerRegistration", model);
        }

        [Authorize]
        public ActionResult ViewAllCustomers(string SearchString)
        {
            
            var customers = context.Customers.OfType<Tiani.P_Bites_Bytes.Models.Customer>().ToList();
            if (!customers.Any())
            {
                ViewBag.ErrorMessage("no customer with that username found");
            }


            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                // If a search string is provided, filter the posts by users
                customers = customers.Where(u => u.EmailAddress.Equals(SearchString.Trim())).ToList();

                if (!customers.Any())
                {
                    TempData["ErrorMessage"] = "no customers with that username found";
                }
            }

            ViewBag.Customers = customers;

            return View();

        }


        [HttpGet]
        public ActionResult EditCustomer(string email)
        {
            if (email == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Customer customer = context.Customers.Find(email);

            if (customer == null)
            {
                return HttpNotFound();
            }

            CustomerViewModel model = new CustomerViewModel
            {
                EmailAddress = customer.EmailAddress,
                Firstname = customer.Firstname,
                Lastname = customer.Lastname,
                Address1 = customer.Address1,
                Address2 = customer.Address2,
                Postcode = customer.Postcode,
                PhoneNumber = customer.PhoneNumber,
                DateOfBirth = customer.DateOfBirth
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCustomer(CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                Customer customer = context.Customers.Find(model.EmailAddress);

                if (customer == null)
                {
                    return HttpNotFound();
                }

                customer.Firstname = model.Firstname;
                customer.Lastname = model.Lastname;
                customer.Address1 = model.Address1;
                customer.Address2 = model.Address2;
                customer.Postcode = model.Postcode;
                customer.PhoneNumber = model.PhoneNumber;

                context.SaveChanges();

                return RedirectToAction("ViewAllCustomers", "Customer");
            }

            return View(model);
        }


 
    }
}

