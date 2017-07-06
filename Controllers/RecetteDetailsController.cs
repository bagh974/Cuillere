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
    public class RecetteDetailsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: RecetteDetails
        public ActionResult Index()
        {
            var recetteDetails = db.RecetteDetails.Include(r => r.Ingredient).Include(r => r.Recette);
            return View(recetteDetails.ToList());
        }

        // GET: RecetteDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecetteDetail recetteDetail = db.RecetteDetails.Find(id);
            if (recetteDetail == null)
            {
                return HttpNotFound();
            }
            return View(recetteDetail);
        }

        // GET: RecetteDetails/Add
        //Permet d'ajouter les ingrédients à la recette qui vient d'être créée
        public ActionResult Add(int? id)
        {
            if (id == null)
            {
                ViewBag.IngredientId = new SelectList(db.Ingredients.OrderBy(x => x.Name), "IngredientId", "Name");
                ViewBag.RecetteId = new SelectList(db.Recettes, "RecetteId", "Name");
                return View();
            }
            ViewBag.IngredientId = new SelectList(db.Ingredients.OrderBy(x => x.Name), "IngredientId", "Name");
            ViewBag.RecetteId = new SelectList(db.Recettes, "RecetteId", "Name", id);
            return View();
        }

        // POST: RecetteDetails/Add
        // Ajout un ingrédient à la recette ou clôture l'enregistrement de cette dernière
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "RecetteDetailId,RecetteId,IngredientId,Quantity,unite")] RecetteDetail recetteDetail, string ajout)
        {
            if (ajout == "Ajouter")
            {
                if (ModelState.IsValid)
                {
                    db.RecetteDetails.Add(recetteDetail);
                    db.SaveChanges();
                    return RedirectToAction("Add", new { id = recetteDetail.RecetteId });
                }
            }
            if (ajout == "Terminer")
            {
                if (ModelState.IsValid)
                {
                    db.RecetteDetails.Add(recetteDetail);
                    db.SaveChanges();
                    return RedirectToAction("Details", "Recettes", new { id = recetteDetail.RecetteId });
                }
            }
            ViewBag.IngredientId = new SelectList(db.Ingredients.OrderBy(x => x.Name), "IngredientId", "Name", recetteDetail.IngredientId);
            ViewBag.RecetteId = new SelectList(db.Recettes.OrderBy(x => x.Name), "RecetteId", "Name", recetteDetail.RecetteId);
            return View(recetteDetail);
        }

        // GET: RecetteDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecetteDetail recetteDetail = db.RecetteDetails.Find(id);
            if (recetteDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.IngredientId = new SelectList(db.Ingredients, "IngredientId", "Name", recetteDetail.IngredientId);
            ViewBag.RecetteId = new SelectList(db.Recettes, "RecetteId", "Name", recetteDetail.RecetteId);
            return View(recetteDetail);
        }

        // POST: RecetteDetails/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RecetteDetailId,RecetteId,IngredientId,Quantity,unite")] RecetteDetail recetteDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(recetteDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IngredientId = new SelectList(db.Ingredients, "IngredientId", "Name", recetteDetail.IngredientId);
            ViewBag.RecetteId = new SelectList(db.Recettes, "RecetteId", "Name", recetteDetail.RecetteId);
            return View(recetteDetail);
        }

        // GET: RecetteDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecetteDetail recetteDetail = db.RecetteDetails.Find(id);
            if (recetteDetail == null)
            {
                return HttpNotFound();
            }
            return View(recetteDetail);
        }

        // POST: RecetteDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RecetteDetail recetteDetail = db.RecetteDetails.Find(id);
            db.RecetteDetails.Remove(recetteDetail);
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
