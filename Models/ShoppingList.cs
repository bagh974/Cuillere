using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cuillere.Models
{
    public partial class ShoppingList
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public string ShoppingListId { get; set; }

        public const string CartSessionKey = "cartId";

        public static ShoppingList GetCart(HttpContextBase context)
        {
            var cart = new ShoppingList();

            cart.ShoppingListId = cart.GetCartId(context);

            return cart;
        }

        public static ShoppingList GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }
        //Ajoute l'ingrédient à la liste si l'unité de mesure n'est pas la même, sinon l'incrémente
        public void AddToCart(RecetteDetail ingredient)
        {
            var cartItem = db.Carts.SingleOrDefault(c => c.CartId == ShoppingListId && c.IngredientId == ingredient.IngredientId && c.unite == ingredient.unite);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    IngredientId = ingredient.IngredientId,
                    CartId = ShoppingListId,
                    Count = ingredient.Quantity,
                    unite = ingredient.unite,
                    DateCreated = DateTime.Now
                };
                db.Carts.Add(cartItem);
            }
            else
            {
                cartItem.Count+= ingredient.Quantity;
             }

            db.SaveChanges();
        }

        public int RemoveFromCart(int id)
        {
            var cartItem = db.Carts.SingleOrDefault(cart => cart.CartId == ShoppingListId && cart.IngredientId == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1 && cartItem.unite == 0)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    db.Carts.Remove(cartItem);
                }

                db.SaveChanges();
            }
            return itemCount;
        }

        public void EmptyCart()
        {
            var cartItems = db.Carts.Where(cart => cart.CartId == ShoppingListId);

            foreach (var cartItem in cartItems)
            {
                db.Carts.Remove(cartItem);
            }
            db.SaveChanges();
        }

        public List<CartItem> GetCartItems()
        {
            return db.Carts.Where(cart => cart.CartId == ShoppingListId).ToList();
        }

        public int GetCount()
        {
            int? count =
                (from cartItems in db.Carts where cartItems.CartId == ShoppingListId select cartItems.Ingredient.Name).Count();

            return count ?? 0;
        }
        
        public int CreateOrder(Order customerOrder)
        {
            decimal orderTotal = 0;

            var cartItems = GetCartItems();

            foreach (var item in cartItems)
            {
                var orderedProduct = new OrderDetail
                {
                    IngredientId = item.IngredientId,
                    OrderDetailId = customerOrder.OrderId,
                    Quantity = item.Count,
                    unite = item.unite
                };

                orderTotal += item.Count;

                db.OrderDetails.Add(orderedProduct);
            }

            db.SaveChanges();

            EmptyCart();

            return customerOrder.OrderId;
        }

        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }

                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }

            return context.Session[CartSessionKey].ToString();
        }

        public void MigrateCart(string userName)
        {
            var ShoppingList = db.Carts.Where(c => c.CartId == ShoppingListId);
            foreach (CartItem item in ShoppingList)
            {
                item.CartId = userName;
            }

            db.SaveChanges();
        }

    }
}