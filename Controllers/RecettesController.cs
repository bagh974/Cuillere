using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using PagedList;
using System.Web.Mvc;
using Cuillere.Models;
using System.Data.Entity.Infrastructure;

namespace Cuillere.Controllers
{
    public class RecettesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Recettes
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.RecetteSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.SaisonSortParm = sortOrder == "Saison" ? "saison_desc" : "Saison";
            ViewBag.CategorySortParm = sortOrder == "Catégorie" ? "category_desc" : "Catégorie";
            ViewBag.TypeSortParm = sortOrder == "Type" ? "type_desc" : "Type";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var recettes = db.Recettes.Include(r => r.Category)
                                      .Include(r => r.Type)
                                      .Include(r => r.Saison);

            //Recherche sur recette, saison ou catégorie
            if (!String.IsNullOrEmpty(searchString))
            {
                recettes = recettes.Where(s => s.Name.Contains(searchString)
                                       || s.Saison.Name.Contains(searchString)
                                       || s.Category.Name.Contains(searchString)
                                       || s.Type.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    recettes = recettes.OrderByDescending(s => s.Name);
                    break;
                case "Saison":
                    recettes = recettes.OrderBy(s => s.Saison.Name);
                    break;
                case "saison_desc":
                    recettes = recettes.OrderByDescending(s => s.Saison.Name);
                    break;
                case "Catégorie":
                    recettes = recettes.OrderBy(s => s.Category.Name);
                    break;
                case "category_desc":
                    recettes = recettes.OrderByDescending(s => s.Category.Name);
                    break;
                case "Type":
                    recettes = recettes.OrderBy(s => s.Type.Name);
                    break;
                case "type_desc":
                    recettes = recettes.OrderByDescending(s => s.Type.Name);
                    break;
                default:
                    recettes = recettes.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(recettes.ToPagedList(pageNumber, pageSize));
        }

        // GET: Recettes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recette recette = db.Recettes.Find(id);
            recette.RecetteDetail = (from i in db.RecetteDetails where i.RecetteId == recette.RecetteId select i).ToList();
            if (recette == null)
            {
                return HttpNotFound();
            }
            return View(recette);
        }

        // GET: Recettes/Create
        public ActionResult Create()
        {
            PopulateCategories();
            PopulateSaisons();
            return View();
        }

        // POST: Recettes/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RecetteId,Name,PReparation,CategoryId,TypeId,SaisonId")] Recette recette, string Type_Name)
        {
            var typeRecette = db.Types.SingleOrDefault(i => i.Name == Type_Name);
            recette.TypeId = typeRecette.TypeId;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Recettes.Add(recette);
                    db.SaveChanges();
                    //redirection vers l'ajout des ingrédients à la recette nouvellement créé
                    return RedirectToAction("Add", "RecetteDetails", new { id = recette.RecetteId });
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            PopulateCategories(recette.CategoryId);
            PopulateSaisons(recette.SaisonId);
            return View(recette);
        }

        public ActionResult Add(int id)
        {
            Recette recette = db.Recettes.Include(r => r.Category)
                                         .Include(r => r.Type)
                                         .Include(r => r.Saison)
                                         .Where(r => r.RecetteId == id)
                                         .Single();
            return RedirectToAction("Add", "RecetteDetails", new { id = recette.RecetteId });
        }
        // GET: Recettes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recette recette = db.Recettes.Include(r=> r.Category)
                                         .Include(r => r.Type)
                                         .Include(r=> r.Saison)
                                         .Where(r=> r.RecetteId == id)
                                         .Single();
            if (recette == null)
            {
                return HttpNotFound();
            }
            PopulateCategories(recette.CategoryId);
            PopulateSaisons(recette.SaisonId);
            return View(recette);
        }

        // POST: Recettes/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id, string Type_Name)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var recetteToUpdate = db.Recettes.Find(id);
            var typeRecette = db.Types.SingleOrDefault(i => i.Name == Type_Name);
            recetteToUpdate.TypeId = typeRecette.TypeId;
            if (TryUpdateModel(recetteToUpdate, "",
               new string[] { "Name", "CategoryId", "TypeId", "SaisonId" }))
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
            PopulateCategories(recetteToUpdate.CategoryId);
            PopulateSaisons(recetteToUpdate.SaisonId);
            return View(recetteToUpdate);
        }

        //Créer la liste des catégories
        private void PopulateCategories(object selectedCategory = null)
        {
            var categoriesQuery = from c in db.Categories
                              orderby c.Name
                              select c;
            ViewBag.CategoryId = new SelectList(categoriesQuery, "CategoryId", "Name", selectedCategory);

        }
        //Liste déroulante en cascade, permet de récupérer les types si la catégorie est Entrées ou Viandes
        //fonction avec JS/Ajax (_layout)
        //ne récupère pas l'ID du type
        //[HttpPost]
        //public ActionResult GetTypes(string catId)
        //{
        //    int categoryId;
        //    List<SelectListItem> typeRequired = new List<SelectListItem>();
        //    if (!string.IsNullOrEmpty(catId))
        //    {
        //        categoryId = Convert.ToInt32(catId);
        //        List<Models.Type> types = db.Types.Where(x => x.CategoryId == categoryId).ToList();
        //        types.ForEach(x =>
        //        {
        //            typeRequired.Add(new SelectListItem { Text = x.Name, Value = x.TypeId.ToString() });
        //        });
        //    }
        //    return Json(typeRequired, JsonRequestBehavior.AllowGet);

        //}
        //Autocomplétion pour les types
        public JsonResult GetTypes(string term = "")
        {
            var objTypelist = db.Types
                            .Where(i => i.Name.ToUpper()
                            .Contains(term.ToUpper()))
                            .Select(i => new { Name = i.Name, ID = i.TypeId })
                            .Distinct().ToList();
            return Json(objTypelist, JsonRequestBehavior.AllowGet);
        }

        private void PopulateSaisons(object selectedSaison = null)
        {
            var saisonsQuery = from s in db.Saisons
                                  orderby s.Name
                                  select s;
            ViewBag.SaisonId = new SelectList(saisonsQuery, "SaisonId", "Name", selectedSaison);
        }

        // GET: Recettes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recette recette = db.Recettes.Find(id);
            if (recette == null)
            {
                return HttpNotFound();
            }
            return View(recette);
        }

        // POST: Recettes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Recette recette = db.Recettes.Find(id);
            db.Recettes.Remove(recette);
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
