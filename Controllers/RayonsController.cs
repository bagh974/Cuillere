﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cuillere.Models;

namespace Cuillere.Controllers
{
    public class RayonsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Rayons
        public ActionResult Index()
        {
            return View(db.Rayons.ToList());
        }

        // GET: Rayons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rayon rayon = db.Rayons.Find(id);
            if (rayon == null)
            {
                return HttpNotFound();
            }
            return View(rayon);
        }

        // GET: Rayons/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rayons/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RayonId,Name")] Rayon rayon)
        {
            if (ModelState.IsValid)
            {
                db.Rayons.Add(rayon);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rayon);
        }

        // GET: Rayons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rayon rayon = db.Rayons.Find(id);
            if (rayon == null)
            {
                return HttpNotFound();
            }
            return View(rayon);
        }

        // POST: Rayons/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RayonId,Name")] Rayon rayon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rayon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rayon);
        }

        // GET: Rayons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rayon rayon = db.Rayons.Find(id);
            if (rayon == null)
            {
                return HttpNotFound();
            }
            return View(rayon);
        }

        // POST: Rayons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rayon rayon = db.Rayons.Find(id);
            db.Rayons.Remove(rayon);
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
