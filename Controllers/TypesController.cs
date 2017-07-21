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
    public class TypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Types
        //public ActionResult Index()
        //{
        //    var types = db.Types.Include(t => t.Category);
        //    return View(types.ToList());
        //}

        // GET: Types/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cuillere.Models.Type type = db.Types.Find(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        // GET: Types/Create
        //public ActionResult Create()
        //{
        //    ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");
        //    return View();
        //}

        // POST: Types/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "TypeId,Name,CategoryId")] Models.Type type)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Types.Add(type);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", type.CategoryId);
        //    return View(type);
        //}

        // GET: Types/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Models.Type type = db.Types.Find(id);
        //    if (type == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", type.CategoryId);
        //    return View(type);
        //}

        // POST: Types/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "TypeId,Name,CategoryId")] Models.Type type)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(type).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", type.CategoryId);
        //    return View(type);
        //}

        // GET: Types/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Models.Type type = db.Types.Find(id);
        //    if (type == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(type);
        //}

        // POST: Types/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Models.Type type = db.Types.Find(id);
        //    db.Types.Remove(type);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
