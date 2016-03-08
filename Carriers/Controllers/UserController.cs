using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Carriers.Models;

namespace Carriers.Controllers
{
    
    public class UserController : Controller
    {
        private CARRIERSEntities db = new CARRIERSEntities();

        // GET: User
        [Authorize(Roles = "A")]
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Login,Password,Role")] Users users)
        {
            if (ModelState.IsValid)
            {
                using (var con = new CARRIERSEntities())
                {
                    var sql = from r
                              in con.Users
                              where (r.Login == users.Login)
                              select r;

                    if (sql.Count() > 0)
                    { 
                        ViewBag.msg = "This name is already used. Please try another one.";
                        return View(users);
                    }
                }

                db.Users.Add(users);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(users);
        }

        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Login,Password,Role")] Users users)
        {
            if (ModelState.IsValid)
            {
                using (var con = new CARRIERSEntities())
                {
                    var sql = from r
                              in con.Users
                              where (r.Login == users.Login && r.Id != users.Id)
                              select r;

                    if (sql.Count() > 0)
                    {
                        ViewBag.msg = "This name is already used. Please try another one.";
                        return View(users);
                    }
                }

                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(users);
        }

        // GET: User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Users users = db.Users.Find(id);

            using (var con = new CARRIERSEntities())
            {
                var sql = from r
                          in con.Rates
                          where r.User == id
                          select r;
                if (sql.Count() > 0)
                {
                    ViewBag.msg = "This User cannot be deleted. There are rates registered for this User.";

                    if (users == null)
                    {
                        return HttpNotFound();
                    }
                    return View(users);
                }

                var sql1 = from q
                           in con.Users
                           where q.Role == "A"
                           select q;

                if (sql1.Count() == 1 && users.Role == "A")
                {
                    ViewBag.msg = "This User cannot be deleted. There must be at least one Admin on the database.";

                    if (users == null)
                    {
                        return HttpNotFound();
                    }
                    return View(users);
                }

            }

            db.Users.Remove(users);
            db.SaveChanges();
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
