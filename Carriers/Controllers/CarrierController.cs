﻿using System;
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
    public class CarrierController : Controller
    {
        private CARRIERSEntities db = new CARRIERSEntities();

        // GET: Carrier
        public ActionResult Index()
        {
            return View(db.Carriers.ToList());
        }

        // GET: Carrier/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Carriers carriers = db.Carriers.Find(id);
            if (carriers == null)
            {
                return HttpNotFound();
            }
            return View(carriers);
        }

        // GET: Carrier/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Carrier/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Models.Carriers carriers)
        {
            if (ModelState.IsValid)
            {
                db.Carriers.Add(carriers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(carriers);
        }

        // GET: Carrier/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Carriers carriers = db.Carriers.Find(id);
            if (carriers == null)
            {
                return HttpNotFound();
            }
            return View(carriers);
        }

        // POST: Carrier/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Models.Carriers carriers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carriers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(carriers);
        }

        // GET: Carrier/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Carriers carriers = db.Carriers.Find(id);
            if (carriers == null)
            {
                return HttpNotFound();
            }
            return View(carriers);
        }

        // POST: Carrier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Models.Carriers carriers = db.Carriers.Find(id);

            using (var con = new CARRIERSEntities())
            {
                var sql = from r
                          in con.Rates
                          where r.Carrier == id
                          select r;
                if (sql.Count() > 0)
                {
                    ViewBag.msg = "This Carrier cannot be deleted. There are rates registered for this Carrier.";

                    if (carriers == null)
                    {
                        return HttpNotFound();
                    }
                    return View(carriers);
                }
            }
            
            db.Carriers.Remove(carriers);
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
