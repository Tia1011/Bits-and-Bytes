
using Stripe.Climate;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Tiani.P_Bites_Bytes.Models;
using Tiani.P_Bites_Bytes.Models.ViewModels;

namespace Tiani.P_Bites_Bytes.Controllers
{
    public class ShopController : Controller
    {
        private BitsAndBytesDbContext context = new BitsAndBytesDbContext();

        public ActionResult Products(string customerEmail, Models.Order order)
        {

            var products = context.Products.ToList();
            foreach (var item in products)
            {
                context.Entry(item).Reload();
            }
            ViewBag.Categories = context.Categories.ToList();


            // Retrieve customer email from orders if not provided
            if (string.IsNullOrEmpty(customerEmail))
            {
                var orderWithEmail = context.Orders.FirstOrDefault(c => c.OrderStatus == OrderStatus.Started && c.EmailAddress != null);
                customerEmail = orderWithEmail?.EmailAddress; // Assign email if found, otherwise, it will be null
                int? orderId = orderWithEmail?.OrderId;
                // Pass customer email to the view
                ViewBag.CustomerEmail = customerEmail;
                ViewBag.OrderId = orderId;
            }

            //int? orderId = order?.OrderId;
           

           
            return View("Products", products);
        }


        public ActionResult ComputerParts()
        {
            if (context == null)
            {
                context = new BitsAndBytesDbContext();
                return RedirectToAction("Error");
            }

            var computerParts = context.Categories.FirstOrDefault(c => c.Name == "ComputerParts");

            // Check if computerParts is null
            if (computerParts == null)
            {
                return RedirectToAction("NotFound", "Error");
            }


            var products = context.Products.Where(p => p.CategoryId == computerParts.CategoryId).ToList();
            ViewBag.Categories = context.Categories.ToList();
            return View("Products", products);


        }

        public ActionResult ComputerSystems()
        {
            var computerSystems = context.Categories.Where(c => c.Name == "computerSystems").SingleOrDefault();


            var products = context.Products.Where(p => p.CategoryId == computerSystems.CategoryId).ToList();
            ViewBag.Categories = context.Categories.ToList();
            return View("Products", products);
        }

        public ActionResult Product(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Models.Product product = context.Products.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }
            var orderWithEmail = context.Orders.FirstOrDefault(c => c.OrderStatus == OrderStatus.Started && c.EmailAddress != null);
            string emailAddress = orderWithEmail?.EmailAddress;
            ViewBag.Email = emailAddress;
            ViewBag.Categories = context.Categories.ToList();
            ViewBag.OrderId = orderWithEmail.OrderId;
            return View(product);
        }



        [HttpPost]
        public ActionResult AddToBasket(string id, int quantity)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                // Check if the context is null and initialize if necessary
                using (var context = new BitsAndBytesDbContext())
                {
                    // Retrieve the product by its ID
                    var product = context.Products.Find(id);

                    // Check if the product exists
                    if (product == null)
                    {
                        return HttpNotFound();
                    }

                    // Retrieve the order with the specified criteria
                    var orderWithEmail = context.Orders
                        .FirstOrDefault(c => c.OrderStatus == OrderStatus.Started && c.EmailAddress != null);

                    // Check if an order with email is found
                    if (orderWithEmail == null)
                    {
                        // Handle the case where the customer email is not found
                        return RedirectToAction("CustomerLogin", "Customer");
                    }

                    // Get the order ID and customer email
                    int orderId = orderWithEmail.OrderId;
                    string emailAddress = orderWithEmail.EmailAddress;

                    // Retrieve the order based on the customer email
                    var order = context.Orders.FirstOrDefault(o => o.EmailAddress == emailAddress && o.OrderStatus == OrderStatus.Started);

                    // Check if the order exists
                    if (order == null)
                    {
                        // Handle the case where the order is not found
                        return HttpNotFound();
                    }

                    // Check if the requested quantity exceeds the available stock
                    if (quantity > product.StockLevel)
                    {
                        // Display an error message
                        ViewBag.ErrorMessage = "Sorry, the requested quantity exceeds the available stock. Only " + product.StockLevel + " items available";
                        ViewBag.Categories = context.Categories.ToList();
                        return View("Product", product);
                    }

                    // Create an OrderLine for the product
                    var orderLine = new OrderLine
                    {
                        OrderId = order.OrderId,
                        ProductId = product.ProductId,
                        Quantity = quantity,
                        Price = product.Price,
                        LineTotal = product.Price * quantity,
                    };

                    // Add the order line to the order
                    order.OrderLines.Add(orderLine);

                    // Update the order total
                    order.OrderTotal += orderLine.LineTotal;

                    // Update the stock level
                    product.UpdateStock(quantity);

                    // Save changes to the database
                    context.SaveChanges();

                    // Redirect the user to the Basket view
                    return RedirectToAction("Basket", new { OrderId = orderId });
                }
            }

           
        

        public Models.Order GetOrder(string customerEmail)
        {
            Models.Order order = context.Orders.FirstOrDefault(o => o.EmailAddress == customerEmail && o.OrderStatus == OrderStatus.Started);

            return order;
        }

        // Action to view the shopping cart
        public ActionResult Basket(int orderId)
        {
            ViewBag.StripePublishableKey = ConfigurationManager.AppSettings["StripePublishableKey"];

            var order = context.Orders.Include(o => o.OrderLines)
                                      .FirstOrDefault(o => o.OrderId == orderId);

            if (order == null)
            {
                // Handle the case where the order is null
                return View();
            }

            ViewBag.OrderItems = order.OrderLines;
            ViewBag.DiscountValue = order.DiscountValue;
            return View("Basket", order);
        }

      
        [HttpPost]
        public ActionResult RemoveFromBasket(string productId, int orderId)
        {
            using (var context = new BitsAndBytesDbContext())
            {
                // Retrieve the order from the database including OrderLines
                var order = context.Orders
                    .Where(o => o.OrderId == orderId)
                    .Include(o => o.OrderLines) // Explicitly load OrderLines
                    .FirstOrDefault();


                if (order != null)
                {
                    // Find the order line corresponding to the product
                    var orderLine = order.OrderLines.FirstOrDefault(ol => ol.ProductId == productId);

                    if (orderLine != null)
                    {
                        // Update the order total before removing the order line
                        order.OrderTotal -= orderLine.LineTotal;

                        // Explicitly remove the order line from the context
                        context.OrderLines.Remove(orderLine);

                        // Save changes to the database
                        context.SaveChanges();
                    }
                    if (order.OrderLines.Count == 0)
                    {
                        order.OrderTotal= 0;
                    }
                }
            }

            // Redirect the user back to the basket view
            return RedirectToAction("Basket", new { orderId = orderId });
        }

        public ActionResult ViewAllCodes()
        {
            var codes = context.SpecialOfferCodes.ToList();
            return View(codes);
        }

        public ActionResult CreateCode()
        {
            var viewModel = new SpecialOfferCodeViewModel
            {
                Customers = context.Customers
                    .Select(c => new SelectListItem
                    {
                        Value = c.EmailAddress,
                        Text = c.EmailAddress
                    })
                    .ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCode(SpecialOfferCodeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Customers = context.Customers
                    .Select(c => new SelectListItem
                    {
                        Value = c.EmailAddress,
                        Text = c.EmailAddress
                    })
                    .ToList();
                return View(viewModel);
            }

            if (viewModel.ValidityDate > DateTime.Today)
            {
                viewModel.OfferInValid = false;
            }

            foreach (var CustomerEmail in viewModel.SelectedCustomers)
            {
                if (CustomerEmail != "false")
                {
                    var specialOfferCode = new SpecialOfferCode
                    {
                        OfferId = Guid.NewGuid().ToString(),
                        OfferCode = viewModel.OfferCode,
                        ValidityDate = viewModel.ValidityDate,
                        PercentOff = viewModel.PercentOff,
                        OfferUsed = false,
                        OfferInValid = viewModel.OfferInValid,
                        CustomerEmail = CustomerEmail // Assign selected customer ID
                    };
                    context.SpecialOfferCodes.Add(specialOfferCode);
                }
                
            }
        context.SaveChanges();
           

            return RedirectToAction("ViewAllCodes", "Shop");
        }


        // GET: SpecialOfferCode/Delete/5
        public ActionResult DeleteCode(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SpecialOfferCode code = context.SpecialOfferCodes.Find(id);
            if (code == null)
            {
                return HttpNotFound();
            }

            return View(code);
        }

        // POST: SpecialOfferCode/Delete/5
        [HttpPost, ActionName("DeleteCode")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCodeConfirmed(string id)
        {
            SpecialOfferCode code = context.SpecialOfferCodes.Find(id);
            context.SpecialOfferCodes.Remove(code);
            context.SaveChanges();
            return RedirectToAction("ViewAllCodes");
        }


        [HttpPost]
        public ActionResult RemoveVoucherCode(string customerEmail)
        {
            // Find the order with the provided customer email
            var order = context.Orders.FirstOrDefault(o => o.EmailAddress == customerEmail && o.OrderStatus == OrderStatus.Started);

            // Reset discount value
            order.DiscountValue = 0;

            // Save changes
            context.SaveChanges();

            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult ApplyVoucherCode(string customerEmail, string voucherCode)
        {
            var offerCode = context.SpecialOfferCodes.FirstOrDefault(o =>
                o.OfferCode == voucherCode &&
                o.CustomerEmail == customerEmail &&
                !o.OfferInValid &&
                !o.OfferUsed);

            if (offerCode != null)
            {
                Models.Order order = context.Orders.FirstOrDefault(o => o.EmailAddress == customerEmail && o.OrderStatus == OrderStatus.Started);

                double discountAmount = CalculateDiscountAmount(order, offerCode.PercentOff);

                // Apply the discount to the order total
                order.OrderTotal -= discountAmount;

                offerCode.OfferUsed = true;
                offerCode.OfferInValid = true;
                order.DiscountValue += discountAmount;
                context.SaveChanges();

                ViewBag.AppliedVoucherCode = offerCode.OfferCode;
                TempData["SuccessMessage"] = offerCode.OfferCode + " has been applied";
                return Json(new { success = true, orderTotal = order.OrderTotal, discountAmount = discountAmount });
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid voucher code or cannot be applied.";
                return Json(new { success = false, message = "Invalid voucher code or cannot be applied." });
            }
        }

        private double CalculateDiscountAmount(Models.Order order,int percentOff)
        {
            // Assuming the discount is a percentage of the total order amount
            double discountPercentage = (double)percentOff / 100;

            // Calculate the discount amount
            double discountAmount = order.OrderTotal * discountPercentage;

            // Round the discount amount to two decimal places
            discountAmount = Math.Round(discountAmount, 2);

            return discountAmount;
        }



    }



}