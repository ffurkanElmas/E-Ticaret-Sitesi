using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Staj.Entity;
using Staj.Identity;
using Staj.Models;

namespace Staj.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> UserManager;
        private RoleManager<ApplicationRole> RoleManager;

        private DataContext db = new DataContext();

        public AccountController()
        {
            var userStore = new UserStore<ApplicationUser>(new IdentityDataContext());
            var roleStore = new RoleStore<ApplicationRole>(new IdentityDataContext());

            UserManager = new UserManager<ApplicationUser>(userStore);
            RoleManager = new RoleManager<ApplicationRole>(roleStore);
        }

        [Authorize]
        public ActionResult Index()
        {
            var orders = db.Orders.Where(i=> i.Username == User.Identity.Name).Select(i=> new UserOrderModel() 
            { 
                Id = i.Id,
                OrderNumber = i.OrderNumber,
                OrderDate = i.OrderDate,
                OrderStatus = i.OrderStatus,
                Total = i.Total
            }).OrderByDescending(i=> i.OrderDate).ToList();
            
            return View(orders);
        }
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Register model)
        {
            
            if (ModelState.IsValid) 
            {
               
                var user = new ApplicationUser();
                user.Name = model.Name;
                user.Surname = model.Surname;
                user.Email = model.Email;
                user.UserName = model.Username;

                // Kullanıcı Adı Kontrolü
                var existingUser = UserManager.FindByName(model.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("usernameError", "Kullanıcı adı zaten mevcut!");
                    return View(model);
                }

                // E-posta kontrolü
                var existingEmail = UserManager.FindByEmail(model.Email);
                if (existingEmail != null)
                {
                    ModelState.AddModelError("emailError", "E-posta zaten mevcut!");
                    return View(model);
                }

                IdentityResult result =  UserManager.Create(user , model.Password);

                if (result.Succeeded)
                {
                    if (RoleManager.RoleExists("User")) 
                    {
                        UserManager.AddToRole(user.Id, "User");
                    }
                    
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("registerError" , "Kullanıcı Oluşturma Hatası!");
                }

            }
            


            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login model , string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.Find(model.Username, model.Password);

                if (user != null)
                {
                    var authManager = HttpContext.GetOwinContext().Authentication;
                    var identityClaims = UserManager.CreateIdentity(user, "ApplicationCookie");
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe
                    };
                    authManager.SignIn(authProperties, identityClaims);

                    if (!string.IsNullOrEmpty(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }

                    var roles = UserManager.GetRoles(user.Id);
                    if (roles.Contains("admin"))
                    {
                        return RedirectToAction("Index", "Product");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı adı veya şifre yanlış!");
                }
            }

            return View(model);
        }

        public ActionResult Logout() 
        {
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();

            return RedirectToAction("Index" , "Home");
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            var entity = db.Orders.Where(i => i.Id == id).Select(i => new OrderDetailsModel()
            {
                OrderId = i.Id,
                OrderNumber = i.OrderNumber,
                Total = i.Total,
                OrderDate = i.OrderDate,
                OrderStatus = i.OrderStatus,
                AddressTitle = i.AddressTitle,
                Address = i.Address,
                City = i.City,
                District = i.District,
                Neighborhood = i.Neighborhood,
                PostCode = i.PostCode,
                OrderLines = i.OrderLines.Select(a=> new OrderLineModel()
                { 
                   ProductId = a.ProductId,
                   ProductName = a.Product.Name,
                   Image = a.Product.Image,
                   Quantity = a.Quantity,
                   Price = a.Price
                }).ToList()

            }).FirstOrDefault();
            
            return View(entity);
        }

        [HttpGet]
        public ActionResult ForgotMyPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotMyPassword(ForgotMyPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = UserManager.FindByEmail(model.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "E-Mail Adresi Bulunamadı!"); 
                return View(model);
            }
            else
            {
                // Şifremi Unuttum İşlemleri Sonra Yapılacak
                // ...
                return RedirectToAction("Login", "Account");
            }

        }
    }

}