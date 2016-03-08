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
    [Authorize]
    public class RateController : Controller
    {
        private CARRIERSEntities db = new CARRIERSEntities();

        // GET: Rate
        public ActionResult Index()
        {
            var rates = db.Rates.Include(r => r.Carriers).Include(r => r.Users).Where(r => r.Users.Login == User.Identity.Name);
            return View(rates.ToList());
        }

        // GET: Rate/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rates rates = db.Rates.Find(id);
            if (rates == null)
            {
                return HttpNotFound();
            }
            return View(rates);
        }

        // GET: Rate/Create
        public ActionResult Create()
        {
            var userId = GetUserId();

            var items = from r
                        in db.Carriers
                        where !db.Rates.Any(x => x.Carrier == r.Id && x.User == userId)
                        select r;

            ViewBag.Carrier = new SelectList(items, "Id", "Name");
            ViewBag.User = new SelectList(db.Users, "Id", "Login");
            return View();
        }

        // POST: Rate/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Carrier,User,Rate")] Rates rates)
        {
            rates.User = GetUserId();

            if (ModelState.IsValid)
            {
                db.Rates.Add(rates);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var items = from r
                        in db.Carriers
                        where !db.Rates.Any(x => x.Carrier == r.Id && x.User == rates.User)
                        select r;

            ViewBag.Carrier = new SelectList(items, "Id", "Name", rates.Carrier);
            ViewBag.User = new SelectList(db.Users, "Id", "Login", rates.User);
            return View(rates);
        }

        // GET: Rate/Edit/5
        public ActionResult Edit(int? id)
        {
            var userId = GetUserId();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rates rates = db.Rates.Find(id);
            if (rates == null)
            {
                return HttpNotFound();
            }

            var items = from r
                        in db.Carriers
                        where !db.Rates.Any(x => x.Carrier == r.Id && x.User == userId && x.Id != rates.Id)
                        select r;

            ViewBag.Carrier = new SelectList(items, "Id", "Name", rates.Carrier);
            ViewBag.User = new SelectList(db.Users, "Id", "Login", rates.User);
            return View(rates);
        }

        // POST: Rate/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Carrier,User,Rate")] Rates rates)
        {
            rates.User = GetUserId();

            if (ModelState.IsValid)
            {
                db.Entry(rates).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var items = from r
                        in db.Carriers
                        where !db.Rates.Any(x => x.Carrier == r.Id && x.User == rates.User && x.Id != rates.Id)
                        select r;

            ViewBag.Carrier = new SelectList(items, "Id", "Name", rates.Carrier);
            ViewBag.User = new SelectList(db.Users, "Id", "Login", rates.User);
            return View(rates);
        }

        // GET: Rate/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rates rates = db.Rates.Find(id);
            if (rates == null)
            {
                return HttpNotFound();
            }
            return View(rates);
        }

        // POST: Rate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rates rates = db.Rates.Find(id);
            db.Rates.Remove(rates);
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

        private int GetUserId()
        {
            using (var con = new CARRIERSEntities())
            {
                var sql = from r
                          in con.Users
                          where (r.Login == User.Identity.Name)
                          select r;

                return sql.FirstOrDefault().Id;
            }
        }
    }
}
