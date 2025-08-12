using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Stripe;
using Stripe.Checkout;
using Tiani.P_Bites_Bytes.Models;

namespace Tiani.P_Bites_Bytes.Controllers
{
    public class PaymentController : Controller
    {
        private readonly BitsAndBytesDbContext context;

        public PaymentController()
        {
            this.context = new BitsAndBytesDbContext();
            StripeConfiguration.ApiKey = ConfigurationManager.AppSettings["StripeSecretKey"];
        }

        public PaymentController(BitsAndBytesDbContext context)
        {
            this.context = context;
            StripeConfiguration.ApiKey = ConfigurationManager.AppSettings["StripeSecretKey"];
        }

        [HttpPost, Route("CreateCheckoutSession/{orderId}")]
        public ActionResult CreateCheckoutSession(int orderId)
        {
            // Retrieve the order details from the database
            Order order = context.Orders.FirstOrDefault(o => o.OrderId == orderId);

            if (order == null)
            {
                return HttpNotFound();
            }

            // Create line items for the session
            List<SessionLineItemOptions> lineItems = order.OrderLines.Select(ol =>
            {
                // Retrieve the product from the database
                Models.Product product = context.Products.FirstOrDefault(p => p.ProductId == ol.ProductId);

                // Ensure product is not null to avoid null reference exceptions
                if (product == null)
                    throw new InvalidOperationException("Product not found");

                // Convert relative image URL to absolute URL
                string imageUrl = Url.Content(product.ImageUrl);
                if (!Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
                {
                    string baseUrl = $"{Request.Url.Scheme}://{Request.Url.Authority}";
                    imageUrl = new Uri(new Uri(baseUrl), imageUrl).ToString();
                }

                // Ensure imageUrl is a valid URL
                if (!Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
                {
                    throw new InvalidOperationException($"Invalid image URL for product {product.ProductId}: {imageUrl}");
                }

                // Create a new SessionLineItemOptions object using the product details
                return new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "gbp",
                        UnitAmountDecimal = (decimal)ol.Price * 100, // Amount in pence
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = product.ProductName,
                            Description = product.Description,
                            Images = new List<string> { imageUrl }
                        },
                    },
                    Quantity = ol.Quantity
                };
            }).ToList();

            // Dynamically generate the base URL with a trailing slash
            string baseUrlUrl = $"{Request.Url.Scheme}://{Request.Url.Authority}{Url.Content("~")}".TrimEnd('/') + "/";

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = baseUrlUrl + Url.Action("Success", "Payment").TrimStart('/'),
                CancelUrl = baseUrlUrl + Url.Action("Cancel", "Payment").TrimStart('/')
            };

            // Log the options for debugging
            System.Diagnostics.Debug.WriteLine("Stripe SessionCreateOptions: ");
            System.Diagnostics.Debug.WriteLine($"SuccessUrl: {options.SuccessUrl}");
            System.Diagnostics.Debug.WriteLine($"CancelUrl: {options.CancelUrl}");
            foreach (var item in options.LineItems)
            {
                System.Diagnostics.Debug.WriteLine($"Product: {item.PriceData.ProductData.Name}, Image: {string.Join(", ", item.PriceData.ProductData.Images)}");
            }

            try
            {
                // Create the session
                var service = new SessionService();
                var session = service.Create(options);

                return Json(new { sessionId = session.Id });
            }
            catch (StripeException ex)
            {
                System.Diagnostics.Debug.WriteLine("StripeException: " + ex.Message);
                return new HttpStatusCodeResult(500, ex.Message);
            }
        }

        [HttpGet]
        public ActionResult Success()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Cancel()
        {
            return View();
        }
    }
}
