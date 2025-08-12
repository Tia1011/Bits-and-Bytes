using Microsoft.AspNet.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Tiani.P_Bites_Bytes.Models;
using Tiani.P_Bites_Bytes.Models.ViewModels;
using System.Net;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;
using System.IO;

namespace Tiani.P_Bites_Bytes.Controllers
{
    public class ManagerController : AccountController
    {

        //here is an instance of the mefisto db contexxt
        private BitsAndBytesDbContext db = new BitsAndBytesDbContext();
        public ManagerController() : base()
        {

        }

        public ManagerController(ApplicationUserManager userManager, ApplicationSignInManager signInManager) // : base(userManager, signInManager)
        {

        }

        // GET: Manager

        [Authorize(Roles = "Manager")]
        public ActionResult Index()
        {

            return View();
        }

        [Authorize]
        public ActionResult ViewAllStaff(string SearchString)
        {
            //get all the staff and order them by registration date
            //var users = db.Users.ToList();

            //get all the users and order them by registration date
            var users = db.Users.OfType<Tiani.P_Bites_Bytes.Models.User>().ToList();
            if (!users.Any())
            {
                ViewBag.ErrorMessage("no users with that username found");
            }


            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                // If a search string is provided, filter the posts by users
                users = users.Where(u => u.UserName.Equals(SearchString.Trim())).ToList();

                if (!users.Any())
                {
                    TempData["ErrorMessage"] = "no users with that username found";
                }
            }


            var managers = users.Where(u => UserManager.IsInRole(u.Id, "Manager")).OrderByDescending(u => u.DateHired).ToList();

            // Retrieve users with role "Sales Assistant"
            var salesAssistants = users.Where(u => UserManager.IsInRole(u.Id, "SalesAssistant")).OrderByDescending(u => u.DateHired).ToList();


            //send the list of ROLES over the index page
            //so we can display them
            ViewBag.Roles = db.Roles.ToList();

            ViewBag.Managers = managers;
            ViewBag.SalesAssistants = salesAssistants;

            return View();

        }


        [HttpGet]
        public ActionResult CreateStaff()
        {
            CreateStaffViewModel staff = new CreateStaffViewModel();

            var roles = db.Roles.Where(r => r.Name == "Manager" || r.Name == "SalesAssistant").Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name
            }).ToList();

            staff.Roles = roles;
            return View(new CreateStaffViewModel
            {
                Roles = roles,
            });

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateStaff(CreateStaffViewModel model)
        {

            if (ModelState.IsValid)
            {

                if (DateTime.TryParse(model.DateHired, out DateTime dateHired))
                {
                    User newStaff = new User
                    {
                        // StaffId = model.StaffId,
                        UserName = model.Email,
                        Email = model.Email,
                        EmailConfirmed = true,
                        Address1 = model.Address1,
                        Address2 = model.Address2,
                        Postcode = model.Postcode,
                        Firstname = model.Firstname,
                        Lastname = model.Lastname,
                        DateHired = dateHired,
                        Salary = model.Salary,
                    };
                    var result = UserManager.Create(newStaff, model.Password);
                    //var result =

                    if (result.Succeeded)
                    {
                        var selectedRoleId = model.Role;

                        if (!string.IsNullOrEmpty(selectedRoleId))//selectedRoleId != null
                        {
                            UserManager.AddToRole(newStaff.Id, selectedRoleId);
                            TempData["AlertMessage"] = "Employee has been created";
                            return RedirectToAction("ViewAllStaff", "Manager");
                        }

                    }

                }
            }
            var roles = db.Roles.Where(r => r.Name == "Manager" || r.Name == "SalesAssistant").Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name
            }).ToList();
            model.Roles = roles;

            return View(model);




        }

        [HttpGet]
        public ActionResult EditStaff(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            //FIND EMPLOYEE IN THE DATA BASE BY ID
            User Staff = db.Users.Find(id) as User;

            string dateHired = Staff.DateHired.ToString("yyyy-MM-dd");
            return View(new EditStaffViewModel
            {

                Address1 = Staff.Address1,
                Address2 = Staff.Address2,
                Postcode = Staff.Postcode,
                Firstname = Staff.Firstname,
                Lastname = Staff.Lastname,
                Email = Staff.Email,
                EmailConfirm = Staff.EmailConfirmed,
                Password = Staff.PasswordHash,
                DateHired = dateHired,
                Role = Staff.StaffRole,

            });

        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> EditStaff(string id,
            [Bind(Include = "FirstName,LastName,Address1,Address2,Postcode,DateHired,SelectedRole")] EditStaffViewModel model)
        {
            if (ModelState.IsValid)
            {

                User staff = (User)await UserManager.FindByIdAsync(id);
                UpdateModel(staff);
                if (DateTime.TryParse(model.DateHired, out DateTime dateHired))
                {
                    staff.DateHired = dateHired;
                }


                IdentityResult result = await UserManager.UpdateAsync(staff);

                if (result.Succeeded)
                {
                    return RedirectToAction("ViewAllStaff", "Manager");
                }
            }
            return View(model);
        }

        public ActionResult Details(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            User user = db.Users.Find(id);

            //displys a specific user details page depending on the role
            if (user == null)
            {
                return HttpNotFound();
            }

            return View("StaffDetails", user);
        }




        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            var currentUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (currentUser.Id == id)
            {
                // If the user tries to delete their own account
                return RedirectToAction("ViewAllStaff", "Manager");
            }


            //FIND EMPLOYEE IN THE DATA BASE BY ID
            User user = db.Users.Find(id) as User;
            return View(new DeleteViewModel
            {

                Address1 = user.Address1,
                Address2 = user.Address2,
                Postcode = user.Postcode,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                DateHired = user.DateHired,
            }); ;


        }
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, DeleteViewModel model)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            //check we are not deleting our own account
            User user = await UserManager.FindByIdAsync(id);//get user id
            //if user does exist
            if (user == null)
            {
                return HttpNotFound();
            }//


            await UserManager.DeleteAsync(user);

            return RedirectToAction("ViewAllStaff", "Manager");
        }

        [HttpGet]
        public async Task<ActionResult> ChangeRole(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //cant change yyour own role
            if (id == User.Identity.GetUserId())
            {
                return RedirectToAction("ViewAllStaff", "Manager");
            }

            //get user by id
            User user = await UserManager.FindByIdAsync(id);

            //get users current role
            string oldRole = (await UserManager.GetRolesAsync(id)).Single();

            //GET ALL the roles from the database and store them asa list selectedistitems
            var items = db.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name,
                Selected = r.Name == oldRole

            }).ToList();

            //build the changeroleviewmodel object including the list of roles
            //and send it tot he view displaying the roles in a drop down list with the users current role display

            return View(new ChangeRoleViewModel
            {

                Username = user.UserName,
                Roles = items,
                OldRole = oldRole,

            });



        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("ChangeRole")]
        public async Task<ActionResult> ChangeRoleConfirmed(string id, [Bind(Include = "Role")] ChangeRoleViewModel model)
        {
            //cant change your own role
            if (id == User.Identity.GetUserId())
            {
                return RedirectToAction("ViewAllStaff", "Manager");
            }



            if (ModelState.IsValid)
            {
                User user = await UserManager.FindByIdAsync(id);//get user id
                string oldRole = (await UserManager.GetRolesAsync(id)).Single();//onlyy ever a single role


                //if current role is the same with the selected role then there is no point to update the database
                if (oldRole == model.Role)
                {
                    return RedirectToAction("ViewAllStaff", "Manager");

                }


                //remove user from the old role first
                await UserManager.RemoveFromRoleAsync(id, oldRole);

                //now we are adding the user to the new role
                await UserManager.AddToRoleAsync(id, model.Role);



                return RedirectToAction("ViewAllStaff", "Manager");
            }

            return View(model);

        }


        public ActionResult ViewAllProducts(string SearchString)
        {


            var products = db.Products.OfType<Tiani.P_Bites_Bytes.Models.Product>().ToList();
            if (!products.Any())
            {
                ViewBag.ErrorMessage("no products with that name found");
            }


            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                // If a search string is provided, filter the posts by users
                products = products.Where(u => u.ProductName.Equals(SearchString.Trim())).ToList();

                if (!products.Any())
                {
                    TempData["ErrorMessage"] = "no products with that username found";
                }
            }

            var computerParts = products.Where(p => p.CategoryId == 1).OrderBy(p => p.ProductId).ToList();

            var computerSystems = products.Where(c => c.CategoryId == 2).OrderBy(u => u.ProductId).ToList();

            ViewBag.Roles = db.Categories.ToList();

            ViewBag.ComputerParts = computerParts;
            ViewBag.ComputerSystems = computerSystems;

            return View();

        }



        [HttpGet]
        public ActionResult CreateProduct()
        {
            CreateProductViewModel product = new CreateProductViewModel();

            var categories = db.Categories.Select(r => new CategoryViewModel
            {
                CategoryId = r.CategoryId,
                CategoryName = r.Name
            }).ToList();

            product.Categories = categories;
            return View(product);
        }

        [HttpPost]
        public ActionResult CreateProduct(CreateProductViewModel model, HttpPostedFileBase imageFile)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Check if image file is provided and its content length is greater than 0
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    // Get the category prefix based on the selected category
                    string categoryPrefix = model.SelectedCategoryId == 1 ? "CP" : "CS";

                    // Get the next available product number for the selected category
                    int nextProductNumber = db.Products.Count(p => p.CategoryId == model.SelectedCategoryId) + 1;

                    // Generate the product ID using the category prefix and product number
                    string productId = categoryPrefix + nextProductNumber.ToString("000");

                    Product oldproduct = db.Products.Find(productId);

                    while (oldproduct != null)
                    { 
                        productId = categoryPrefix + ((nextProductNumber + 1).ToString("000"));
                        oldproduct = db.Products.Find(productId);
                        nextProductNumber += 1;
                    }
                    
                    
                    
                    // Get the file name and extension
                    string fileName = Path.GetFileName(imageFile.FileName);
                    string extension = Path.GetExtension(fileName);

                    // Ensure the extension is valid (you may add additional checks here)
                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                    {
                        // Save the image to the Images folder
                        string imagePath = Path.Combine(Server.MapPath("~/Images/Products/"), productId + extension);
                        imageFile.SaveAs(imagePath);

                        // Create a new Product object
                        Product newProduct = new Product
                        {
                            ProductId = productId,
                            ProductName = model.ProductName,
                            Description = model.Description,
                            Price = model.Price,
                            StockLevel = model.StockLevel,
                            ImageUrl = "/Images/Products/" + productId + extension, // Store the relative path of the image
                            StockUpdatedOn = DateTime.Now,
                            CategoryId = model.SelectedCategoryId
                        };

                        // Save the new product to the database
                        db.Products.Add(newProduct);
                        db.SaveChanges();

                        TempData["AlertMessage"] = "Product has been created";
                        return RedirectToAction("ViewAllProducts", "Manager");
                    }
                    else
                    {
                        // Add error message if the file extension is not supported
                        ModelState.AddModelError("", "Only JPEG and PNG images are allowed.");
                    }
                }
                else
                {
                    // Add error message if no image file is provided
                    ModelState.AddModelError("", "Please select an image.");
                }
            }

          

            return View(model);
        }




        [HttpGet]
        public async Task<ActionResult> EditProduct(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Find the product in the database by ID
            Product product = db.Products.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            // Create an EditProductViewModel instance to pass data to the view
            EditProductViewModel model = new EditProductViewModel
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Description = product.Description,
                Price = product.Price,
                StockLevel = product.StockLevel,
                ImageUrl = product.ImageUrl,
            };


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProduct(EditProductViewModel model, HttpPostedFileBase ImageFile, string id)
        {

            if (ModelState.IsValid)
            {
                // Find the product in the database by ID
                Product product = db.Products.Find(id);

                if (product == null)
                {
                    return HttpNotFound();
                }

                // Update product properties with values from the model
                product.ProductName = model.ProductName;
                product.Description = model.Description;
                product.Price = model.Price;
                product.StockLevel = model.StockLevel;
                // Check if a new image file is provided
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    // Delete the existing image file
                    string existingImagePath = Server.MapPath(product.ImageUrl);
                    if (System.IO.File.Exists(existingImagePath))
                    {
                        System.IO.File.Delete(existingImagePath);
                    }

                    // Get the file extension
                    string extension = Path.GetExtension(ImageFile.FileName).ToLower();

                    // Ensure the extension is valid (you may add additional checks here)
                    if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
                    {
                        // Generate a new file name and save the image to the Images folder
                        string productId = product.ProductId;
                        string fileName = productId + extension;
                        string imagePath = Path.Combine(Server.MapPath("~/Images/Products/"), fileName);
                        ImageFile.SaveAs(imagePath);

                        // Update the ImageUrl property of the product
                        product.ImageUrl = "/Images/Products/" + fileName;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Only JPEG and PNG images are allowed.");
                        return View(model);
                    }
                }

                // Save changes to the database
                db.SaveChanges();
                TempData["AlertMessage"] = "Product has been updated";
                return RedirectToAction("ViewAllProducts", "Manager");
            }

          
            return View(model);
        }


        public ActionResult ProductDetails(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = db.Products.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        [HttpGet]
        public async Task<ActionResult> DeleteProduct(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = db.Products.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }

        
            return View(new DeleteProductViewModel
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Description = product.Description,
                Price = product.Price,
                StockLevel = product.StockLevel,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteProduct(string id, DeleteProductViewModel model)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = db.Products.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            db.Products.Remove(product);
            db.SaveChanges();

            TempData["AlertMessage"] = "Product has been deleted";
            return RedirectToAction("ViewAllProducts", "Manager");
        }


        [HttpGet]
        public async Task<ActionResult> UpdateStockLevel(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Find the product in the database by ID
            Product product = db.Products.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            // Create an editstockviewmodel instance to pass data to the view
            UpdateStockViewModel model = new UpdateStockViewModel
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                StockLevel = product.StockLevel,
            };


            return View(model);
        }

        private ActionResult HttpNotFound()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStockLevel(EditProductViewModel model, HttpPostedFileBase ImageFile, string id)
        {

            if (ModelState.IsValid)
            {
                // Find the product in the database by ID
                Product product = db.Products.Find(id);

                if (product == null)
                {
                    return HttpNotFound();
                }


                product.StockLevel = model.StockLevel;



                // Save changes to the database
                db.SaveChanges();
                TempData["AlertMessage"] = "Stock level has been updated";
                return RedirectToAction("ViewAllProducts", "Manager");
            }


            return View(model);
        }

    }



}
