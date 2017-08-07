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
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(string searchString)
        {
            var recettes = db.Recettes.Include(r => r.Category)
                                      .Include(r => r.Type)
                                      .Include(r => r.Saison);

            if (!String.IsNullOrEmpty(searchString))
            {
                recettes = recettes.Where(s => s.Name.Contains(searchString)
                                       || s.Saison.Name.Contains(searchString)
                                       || s.Category.Name.Contains(searchString)
                                       || s.Type.Name.Contains(searchString));
            }
            return View(recettes);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        //[ChildActionOnly]
        //public ActionResult CatMenu()
        //{
        //    List<Category> catMenu = db.Categories.ToList();
        //    return PartialView(catMenu);
        //}
    }
}