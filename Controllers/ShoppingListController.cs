using Cuillere.Models;
using Cuillere.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cuillere.Controllers
{
    public class ShoppingListController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        public ActionResult Index()
        {
            var cart = ShoppingList.GetCart(this.HttpContext);

            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems()
            };

            return View(viewModel);
            //return PartialView(viewModel);
        }

        public ActionResult AddToCart(int id)
        {
            var addedProduct = db.RecetteDetails.Single(product => product.RecetteDetailId == id);
            var CurrentRecette = addedProduct.RecetteId;
            var cart = ShoppingList.GetCart(this.HttpContext);

            cart.AddToCart(addedProduct);

            return RedirectToAction("Details", "Recettes", new { id = CurrentRecette });
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            var cart = ShoppingList.GetCart(this.HttpContext);

            string productName = db.Carts.FirstOrDefault(item => item.IngredientId == id).Ingredient.Name;

            int itemCount = cart.RemoveFromCart(id);

            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(productName) + " has been removed from your shopping cart",
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };

            return Json(results);
        }

        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingList.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }

        [ChildActionOnly]
        public ActionResult shoppinglist()
        {
            var cart = ShoppingList.GetCart(this.HttpContext);
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems()
            };

            return PartialView("shoppinglist", viewModel);
        }
    }
}