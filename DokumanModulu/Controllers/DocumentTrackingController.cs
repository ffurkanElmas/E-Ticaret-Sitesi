using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Net;
using System.Web.Mvc;
using System.Web;
using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using DokumanModulu.Entity;
using DokumanModulu.Identity;
using DokumanModulu.Models;
using System.Linq;

namespace DokumanModulu.Controllers
{
    public class DocumentTrackingController : Controller
    {
        private DataContext db = new DataContext();
        private IdentityDataContext identityDb = new IdentityDataContext();

        [Authorize]
        public ActionResult Index()
        {
            var currentUserId = User.Identity.GetUserId();
            var userManager = new UserManager<User>(new UserStore<User>(identityDb));
            var isAdmin = userManager.IsInRole(currentUserId, "admin");

            List<DocumentTracking> documents;
            if (isAdmin)
            {
                documents = db.Documents.ToList();
            }
            else
            {
                documents = db.Documents
                    .Where(d => d.AllowedUsers.Contains(currentUserId))
                    .ToList();
            }

            return View(documents);
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DocumentTracking documentTracking, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var uploadsDir = Server.MapPath("~/Uploads/");

                if (!Directory.Exists(uploadsDir))
                {
                    Directory.CreateDirectory(uploadsDir);
                }

                documentTracking.Path = "/Uploads/" + fileName;
                var path = Path.Combine(uploadsDir, fileName);
                file.SaveAs(path);
            }
            else
            {
                ModelState.AddModelError("Path", "Dosya yolu alanı gereklidir.");
            }

            if (ModelState.IsValid)
            {
                documentTracking.CreatedDate = DateTime.Now;
                documentTracking.CreatedUserId = User.Identity.GetUserId();
                documentTracking.AllowedUsers = User.Identity.GetUserId(); // Sadece kendini izinli yap

                db.Documents.Add(documentTracking);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(documentTracking);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentTracking documentTracking = db.Documents.Find(id);
            if (documentTracking == null)
            {
                return HttpNotFound();
            }

            var users = identityDb.Users.ToList();
            var userRoles = new Dictionary<string, string>();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(identityDb));
            var userManager = new UserManager<User>(new UserStore<User>(identityDb));

            foreach (var user in users)
            {
                var roles = userManager.GetRoles(user.Id);
                userRoles[user.Id] = roles.Count > 0 ? string.Join(", ", roles) : "No Role";
            }

            // Admin kullanıcıları filtreleyin
            var nonAdminUsers = users.Where(u => !userManager.IsInRole(u.Id, "admin")).ToList();

            ViewBag.UserList = nonAdminUsers;
            ViewBag.UserRoles = userRoles;
            ViewBag.SelectedUsers = (documentTracking.AllowedUsers?.Split(',') ?? new string[] { }).ToList(); // List<string> olarak ayarla

            return View(documentTracking);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,Path,CreatedUserId,CreatedDate")] DocumentTracking documentTracking, string[] selectedUsers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(documentTracking).State = EntityState.Modified;
                db.SaveChanges();

                // İzinli kullanıcıyı güncelle
                var allowedUsers = selectedUsers != null ? string.Join(",", selectedUsers) : string.Empty;
                documentTracking.AllowedUsers = allowedUsers;
                db.Entry(documentTracking).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            var users = identityDb.Users.ToList();
            var userRoles = new Dictionary<string, string>();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(identityDb));
            var userManager = new UserManager<User>(new UserStore<User>(identityDb));

            foreach (var user in users)
            {
                var roles = userManager.GetRoles(user.Id);
                userRoles[user.Id] = roles.Count > 0 ? string.Join(", ", roles) : "No Role";
            }

            // Admin kullanıcıları filtreleyin
            var nonAdminUsers = users.Where(u => !userManager.IsInRole(u.Id, "admin")).ToList();

            ViewBag.UserList = nonAdminUsers;
            ViewBag.UserRoles = userRoles;
            ViewBag.SelectedUsers = documentTracking.AllowedUsers?.Split(',') ?? new string[] { };

            return View(documentTracking);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            DocumentTracking documentTracking = db.Documents.Find(id);
            if (documentTracking != null)
            {
                string filePath = Server.MapPath(documentTracking.Path);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                db.Documents.Remove(documentTracking);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                identityDb.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize]
        public ActionResult ViewFile(int id)
        {
            var document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }

            var currentUserId = User.Identity.GetUserId();
            var userManager = new UserManager<User>(new UserStore<User>(identityDb));
            var isAdmin = userManager.IsInRole(currentUserId, "admin");

            var allowedUsers = document.AllowedUsers?.Split(',') ?? new string[] { };

            if (!isAdmin && !allowedUsers.Contains(currentUserId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Bu dosyayı görüntüleme izniniz yok.");
            }

            string filePath = Server.MapPath(document.Path);
            string contentType = MimeMapping.GetMimeMapping(filePath);

            Response.AppendHeader("Content-Disposition", "inline; filename=" + Path.GetFileName(filePath));
            return File(filePath, contentType);
        }
    }
}
