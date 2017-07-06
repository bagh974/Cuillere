using System;
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
    public class SaisonsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Saisons
        public ActionResult Index()
        {
            return View(db.Saisons.ToList());
        }

        // GET: Saisons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Saison saison = db.Saisons.Find(id);
            if (saison == null)
            {
                return HttpNotFound();
            }
            return View(saison);
        }

        // GET: Saisons/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Saisons/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SaisonId,Name")] Saison saison)
        {
            if (ModelState.IsValid)
            {
                db.Saisons.Add(saison);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(saison);
        }

        // GET: Saisons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Saison saison = db.Saisons.Find(id);
            if (saison == null)
            {
                return HttpNotFound();
            }
            return View(saison);
        }

        // POST: Saisons/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SaisonId,Name")] Saison saison)
        {
            if (ModelState.IsValid)
            {
                db.Entry(saison).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(saison);
        }

        // GET: Saisons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Saison saison = db.Saisons.Find(id);
            if (saison == null)
            {
                return HttpNotFound();
            }
            return View(saison);
        }

        // POST: Saisons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Saison saison = db.Saisons.Find(id);
            db.Saisons.Remove(saison);
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
