using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using DokumanModulu.Entity;
using DokumanModulu.Identity;
using DokumanModulu.Models;

namespace DokumanYuklemeModulu.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> UserManager;
        private RoleManager<IdentityRole> RoleManager;

        public AccountController()
        {
            var userStore = new UserStore<User>(new IdentityDataContext());
            var roleStore = new RoleStore<IdentityRole>(new IdentityDataContext());

            UserManager = new UserManager<User>(userStore);
            RoleManager = new RoleManager<IdentityRole>(roleStore);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Register()
        {
            ViewBag.Roles = new SelectList(RoleManager.Roles.ToList(), "Name", "Name");
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Register model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    UserName = model.Email
                };

                var existingEmail = UserManager.FindByEmail(model.Email);
                if (existingEmail != null)
                {
                    ModelState.AddModelError("emailError", "E-posta zaten mevcut!");
                    ViewBag.Roles = new SelectList(RoleManager.Roles.ToList(), "Name", "Name");
                    return View(model);
                }

                IdentityResult result = UserManager.Create(user, model.Password);

                if (result.Succeeded)
                {
                    if (RoleManager.RoleExists(model.Role))
                    {
                        UserManager.AddToRole(user.Id, model.Role);
                    }

                    return RedirectToAction("Index", "Account");
                }
                else
                {
                    ModelState.AddModelError("registerError", "Kullanıcı Oluşturma Hatası!");
                }
            }

            ViewBag.Roles = new SelectList(RoleManager.Roles.ToList(), "Name", "Name");
            return View(model);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login model, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.FindByEmail(model.Email);

                if (user != null && UserManager.CheckPassword(user, model.Password))
                {
                    var authManager = HttpContext.GetOwinContext().Authentication;
                    var identityClaims = UserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = false
                    };
                    authManager.SignIn(authProperties, identityClaims);

                    if (!string.IsNullOrEmpty(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }

                    var roles = UserManager.GetRoles(user.Id);
                    if (roles.Contains("admin"))
                    {
                        return RedirectToAction("Index", "DocumentTracking");
                    }
                    else
                    {
                        return RedirectToAction("Index", "DocumentTracking");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "E-posta veya şifre yanlış!");
                }
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            using (var context = new IdentityDataContext())
            {
                var users = context.Users.ToList();
                var userRoles = new Dictionary<string, string>();

                foreach (var user in users)
                {
                    var roles = UserManager.GetRoles(user.Id);
                    userRoles[user.Id] = string.Join(", ", roles);
                }

                ViewBag.UserRoles = userRoles;
                return View(users);
            }
        }

        [Authorize(Roles = "admin")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var user = UserManager.FindById(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var currentRole = UserManager.GetRoles(user.Id).FirstOrDefault();
            ViewBag.Roles = new SelectList(RoleManager.Roles.ToList(), "Name", "Name", currentRole);
            return View(user);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User model, string role)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.FindById(model.Id);
                if (user != null)
                {
                    user.Name = model.Name;
                    user.Surname = model.Surname;
                    user.Email = model.Email;
                    user.PhoneNumber = model.PhoneNumber;
                    user.UserName = model.Email;

                    var currentRole = UserManager.GetRoles(user.Id).FirstOrDefault();
                    if (currentRole != role)
                    {
                        UserManager.RemoveFromRole(user.Id, currentRole);
                        UserManager.AddToRole(user.Id, role);
                    }

                    IdentityResult result = UserManager.Update(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Kullanıcı güncelleme hatası!");
                    }
                }
            }
            ViewBag.Roles = new SelectList(RoleManager.Roles.ToList(), "Name", "Name", role);
            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            var user = UserManager.FindById(id);
            if (user != null)
            {
                IdentityResult result = UserManager.Delete(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı silme hatası!");
                }
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = UserManager.FindById(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var role = UserManager.GetRoles(user.Id).FirstOrDefault();
            ViewBag.Role = role;

            return PartialView("_UserDetails", user);
        }

    }
}
