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
    public class RecettesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Recettes
        public ActionResult Index(string sortOrder)
        {
            ViewBag.RecetteSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.SaisonSortParm = sortOrder == "Saison" ? "saison_desc" : "Saison";
            ViewBag.CategorySortParm = sortOrder == "Catégorie" ? "category_desc" : "Catégorie";
            var recettes = db.Recettes.Include(r => r.Category).Include(r => r.Saison);
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
                default:
                    recettes = recettes.OrderBy(s => s.Name);
                    break;
            }
            
            return View(recettes.ToList());
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
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");
            ViewBag.SaisonId = new SelectList(db.Saisons, "SaisonId", "Name");
            return View();
        }

        // POST: Recettes/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RecetteId,Name,CategoryId,SaisonId")] Recette recette)
        {
            if (ModelState.IsValid)
            {
                db.Recettes.Add(recette);
                db.SaveChanges();
                //redirection vers l'ajout des ingrédients à la recette nouvellement créé
                return RedirectToAction("Add", "RecetteDetails", new { id = recette.RecetteId });
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", recette.CategoryId);
            ViewBag.SaisonId = new SelectList(db.Saisons, "SaisonId", "Name", recette.SaisonId);
            return View(recette);
        }

        // GET: Recettes/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", recette.CategoryId);
            ViewBag.SaisonId = new SelectList(db.Saisons, "SaisonId", "Name", recette.SaisonId);
            return View(recette);
        }

        // POST: Recettes/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RecetteId,Name,CategoryId,SaisonId")] Recette recette)
        {
            if (ModelState.IsValid)
            {
                db.Entry(recette).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", recette.CategoryId);
            ViewBag.SaisonId = new SelectList(db.Saisons, "SaisonId", "Name", recette.SaisonId);
            return View(recette);
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
