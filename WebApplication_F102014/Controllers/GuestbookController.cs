using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApplication_F102014.Models;

namespace WebApplication_F102014.Controllers
{
    public class GuestbookController : Controller
    {
        private GuestbookContext db = new GuestbookContext();

        // GET: Guestbook
        public ActionResult Index()
        {
            ViewData["hasPermission"] = UsersController.currentUser != null;
            var mostRecentEntries = (from entry in db.Entries
                                     orderby entry.DateAdded descending
                                     select entry).Take(20);

            return View(mostRecentEntries.ToList());
        }

        // GET: Guestbook/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuestbookEntry guestbookEntry = db.Entries.Find(id);
            if (guestbookEntry == null)
            {
                return HttpNotFound();
            }

            return View(guestbookEntry);
        }

        // GET: Guestbook/Create
        public ActionResult Create()
        {
            //the user can create a guestbook entry, only if he is logged in
            if (UsersController.currentUser != null)
            {
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }

        // POST: Guestbook/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Message,DateAdded,UserId")] GuestbookEntry guestbookEntry)
        {
            //redirect the user if he is not logged in
            if (UsersController.currentUser == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                guestbookEntry.UserId = UsersController.currentUser.Id;
                guestbookEntry.Name = UsersController.currentUser.Username;
                guestbookEntry.DateAdded = DateTime.Now;
                db.Entries.Add(guestbookEntry);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(guestbookEntry);
        }

        // GET: Guestbook/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null ||
                db.Entries.Find(id) == null)
            {
                return RedirectToAction("Index", "Home");
            }

            //if the user is logged in, he can edit only his entries
            GuestbookEntry guestbookEntry = db.Entries.Find(id);
            if (guestbookEntry == null ||
                !(UsersController.currentUser != null && UsersController.currentUser.Username == guestbookEntry.Name))
            {
                return RedirectToAction("Index", "Home");
            }

            return View(guestbookEntry);
        }

        // POST: Guestbook/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Message,DateAdded,UserId")] GuestbookEntry guestbookEntry)
        {
            GuestbookEntry entry = db.Entries.Find(guestbookEntry.Id);
            entry.Message = guestbookEntry.Message;

            if (ModelState.IsValid)
            {
                db.Entry(entry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(entry);
        }

        // GET: Guestbook/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null ||
                db.Entries.Find(id) == null)
            {
                return RedirectToAction("Index", "Home");
            }

            GuestbookEntry guestbookEntry = db.Entries.Find(id);
            //if the user is logged in, he can delete only his profile
            if (UsersController.currentUser != null && UsersController.currentUser.Username == guestbookEntry.Name)
            {
                return View(guestbookEntry);
            }
            else return RedirectToAction("Index", "Home");
        }

        // POST: Guestbook/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GuestbookEntry guestbookEntry = db.Entries.Find(id);

            //extra safety
            if (UsersController.currentUser == null || UsersController.currentUser.Username != guestbookEntry.Name)
            {
                return View("Index");
            }

            if (guestbookEntry.Name == UsersController.currentUser.Username)
            {
                db.Entries.Remove(guestbookEntry);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
