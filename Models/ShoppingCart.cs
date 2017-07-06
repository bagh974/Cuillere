using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Cuillere.Models
{
    public class ShoppingCart
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly string _shoppingCartId;

        private ShoppingCart(ApplicationDbContext dbContext, string id)
        {
            _dbContext = dbContext;
            _shoppingCartId = id;
        }

        public static ShoppingCart GetCart(ApplicationDbContext db, string cartId)
            => new ShoppingCart(db, cartId);

        public async Task AddToCart(Ingredient ingredient)
        {
            // Get the matching cart and ingredient instances
            var cartItem = await _dbContext.CartItems.SingleOrDefaultAsync(
                c => c.CartId == _shoppingCartId
                && c.IngredientId == ingredient.IngredientId);

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new CartItem
                {
                    IngredientId = ingredient.IngredientId,
                    CartId = _shoppingCartId,
                    Count = 1
                };

                _dbContext.CartItems.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, then add one to the quantity
                cartItem.Count++;
            }
        }

        public int RemoveFromCart(int id)
        {
            // Get the cart
            var cartItem = _dbContext.CartItems.SingleOrDefault(
                cart => cart.CartId == _shoppingCartId
                && cart.CartItemId == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    _dbContext.CartItems.Remove(cartItem);
                }
            }

            return itemCount;
        }

        public async Task EmptyCart()
        {
            var cartItems = await _dbContext
                .CartItems
                .Where(cart => cart.CartId == _shoppingCartId)
                .ToArrayAsync();

            _dbContext.CartItems.RemoveRange(cartItems);
        }

        public Task<List<CartItem>> GetCartItems()
        {
            return _dbContext
                .CartItems
                .Where(cart => cart.CartId == _shoppingCartId)
                .Include(c => c.Ingredient)
                .ToListAsync();
        }

        public Task<List<string>> GetCartIngredientNames()
        {
            return _dbContext
                .CartItems
                .Where(cart => cart.CartId == _shoppingCartId)
                .Select(c => c.Ingredient.Name)
                .OrderBy(n => n)
                .ToListAsync();
        }

        public Task<int> GetCount()
        {
            // Get the count of each item in the cart and sum them up
            return _dbContext
                .CartItems
                .Where(c => c.CartId == _shoppingCartId)
                .Select(c => c.Count)
                .SumAsync();
        }

        public async Task<int> CreateOrder(Order order)
        {
            var cartItems = await GetCartItems();

            // Iterate over the items in the cart, adding the order details for each
            foreach (var item in cartItems)
            {
                //var ingredient = _db.Ingredients.Find(item.IngredientId);
                var ingredient = await _dbContext.Ingredients.SingleAsync(a => a.IngredientId == item.IngredientId);

                var orderDetail = new OrderDetail
                {
                    IngredientId = item.IngredientId,
                    OrderId = order.OrderId,
                    Quantity = item.Count,
                };

                _dbContext.OrderDetails.Add(orderDetail);
            }

            // Empty the shopping cart
            await EmptyCart();

            // Return the OrderId as the confirmation number
            return order.OrderId;
        }
    }
}