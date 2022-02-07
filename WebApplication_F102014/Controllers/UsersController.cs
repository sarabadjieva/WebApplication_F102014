using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using WebApplication_F102014.Models;

namespace WebApplication_F102014.Controllers
{
    public class UsersController : Controller
    {
        public static User currentUser = null;
        private UsersContext db = new UsersContext();

        #region Login

        // GET: Users/Login
        public ActionResult Login()
        {
            if (currentUser != null)
            {
                return RedirectToAction("../Home/Index");
            }

            return View();
        }

        // POST: Users/Registration
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Id,Username,Password")] User user)
        {
            User dbUser = db.Users.ToList().Find(x => x.Username == user.Username);

            if (dbUser != null)
            {
                if (dbUser.Password != user.Password)
                {
                    ModelState.AddModelError("Password", "Password incorrect");
                }
            }
            else
            {
                ModelState.AddModelError("Password", "User does not exist");
            }

            if (ModelState.IsValid)
            {
                currentUser = dbUser;
                return RedirectToAction("../Home/Index");
            }

            return View();
        }

        #endregion

        #region Registration

        // GET: Users/Registration
        public ActionResult Registration()
        {
            return View();
        }

        // POST: Users/Registration
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Include = "Id,Username,Password")] User user)
        {
            if (db.Users.Count() > 0)
            {
                foreach (User u in db.Users)
                {
                    if (u.Username == user.Username)
                    {
                        ModelState.AddModelError("Username", "Username already exists");
                        //return RedirectToAction("Create");
                        break;
                    }
                }
            }

            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("../Home/Index");
            }

            return View(user);
        }

        #endregion

        public ActionResult Logout()
        {
            currentUser = null;
            return RedirectToAction("../Home/Index");
        }


        #region Manage Profile

        //GET:Users/Profile
        new public ActionResult Profile()
        {
            if (currentUser == null)
            {
                return RedirectToAction("../Home/Index");
            }
            return View(currentUser);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null || id != currentUser.Id)
            {
                return RedirectToAction("../Home/Index");
            }

            return View(currentUser);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Password")] EditableUser user)
        {
            currentUser.Password = user.Password;

            if (ModelState.IsValid)
            {
                db.Entry(currentUser).State = EntityState.Modified;
                db.SaveChanges();
             
                return RedirectToAction("../Home/Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete()
        {
            User user = db.Users.Find(currentUser.Id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed()
        {
            User user = db.Users.Find(currentUser.Id);
            db.Users.Remove(user);
            db.SaveChanges();
            currentUser = null;
            return RedirectToAction("../Home/Index");
        }

        #endregion

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
