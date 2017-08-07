using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cuillere.Models;
using System.Data.Entity.Infrastructure;

namespace Cuillere.Controllers
{
    public class IngredientsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Ingredients
        public ActionResult Index()
        {
            var ingredients = db.Ingredients.Include(i => i.Rayon);
            ingredients = ingredients.OrderBy(i => i.Rayon.Name);
            return View(ingredients.ToList());
        }

        // GET: Ingredients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ingredient ingredient = db.Ingredients.Find(id);
            if (ingredient == null)
            {
                return HttpNotFound();
            }
            return View(ingredient);
        }

        // GET: Ingredients/Create
        public ActionResult Create()
        {
            //ViewBag.RayonId = new SelectList(db.Rayons, "RayonId", "Name");
            PopulateRayons();
            return View();
        }

        // POST: Ingredients/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IngredientId,Name,RayonId")] Ingredient ingredient)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Ingredients.Add(ingredient);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            PopulateRayons(ingredient.RayonId);

            //ViewBag.RayonId = new SelectList(db.Rayons, "RayonId", "Name", ingredient.RayonId);
            return View(ingredient);
        }

        // GET: Ingredients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Ingredient ingredient = db.Ingredients.Find(id);
            Ingredient ingredient = db.Ingredients.Include(i=> i.Rayon)
                                                  .Where(i=> i.IngredientId == id)
                                                  .Single();
            if (ingredient == null)
            {
                return HttpNotFound();
            }
            //ViewBag.RayonId = new SelectList(db.Rayons, "RayonId", "Name", ingredient.RayonId);
            PopulateRayons(ingredient.RayonId);
            return View(ingredient);
        }

        // POST: Ingredients/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ingredientToUpdate = db.Ingredients.Find(id);
            if (TryUpdateModel(ingredientToUpdate, "",
               new string[] { "Name", "RayonId" }))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            PopulateRayons(ingredientToUpdate.RayonId);
            return View(ingredientToUpdate);
        }

        private void PopulateRayons(object selectedRayon = null)
        {
            var rayonsQuery = from r in db.Rayons
                                   orderby r.Name
                                   select r;
            ViewBag.RayonId = new SelectList(rayonsQuery, "RayonId", "Name", selectedRayon);
        }
        // GET: Ingredients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ingredient ingredient = db.Ingredients.Find(id);
            if (ingredient == null)
            {
                return HttpNotFound();
            }
            return View(ingredient);
        }

        // POST: Ingredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ingredient ingredient = db.Ingredients.Find(id);
            db.Ingredients.Remove(ingredient);
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
