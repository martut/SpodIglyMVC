using SpodIglyMVC.DAL;
using SpodIglyMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpodIglyMVC.Imfrastructure
{
    public class ShoppingCartManager
    {
        private StoreContext Db;

        private ISessionManager session;

        public const string CartSessionKey = "CartData";

        public ShoppingCartManager(ISessionManager session, StoreContext Db)
        {
            this.session = session;
            this.Db = Db;
        }

        public void AddToCart(int albumid)
        {
            var cart = this.GetCart();

            var cartItem = cart.Find(c => c.Album.AlbumId == albumid);

            if (cartItem != null)
                cartItem.Quantity++;
            else
            {
                var albumToAdd = Db.Album.Where(a => a.AlbumId == albumid).SingleOrDefault();
                if (albumToAdd != null)
                {
                    var newCartItem = new CartItem()
                    {
                        Album = albumToAdd,
                        Quantity = 1,
                        TotalPrice = albumToAdd.Price
                    };

                    cart.Add(newCartItem);
                }
            }

            session.Set(CartSessionKey, cart);
        }
        public List<CartItem> GetCart()
        {
            List<CartItem> cart;

            if (session.Get<List<CartItem>>(CartSessionKey) == null)
            {
                cart = new List<CartItem>();
            }
            else
            {
                cart = session.Get<List<CartItem>>(CartSessionKey) as List<CartItem>;
            }
            return cart;
        }

        public int RemoveFromCart(int albumid)
        {
            var cart = this.GetCart();

            var cartItem = cart.Find(c => c.Album.AlbumId == albumid);

            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                    return cartItem.Quantity;
                }
                else
                {
                    cart.Remove(cartItem);
                }
            }
            return 0;
        }

        public decimal GetCartTotalPrice()
        {
            var cart = this.GetCart();

            return cart.Sum(c => (c.Quantity * c.Album.Price));
        }

        public int GetCartItemCount()
        {
            var cart = this.GetCart();
            int count = cart.Sum(c => c.Quantity);

            return count;
        }

        public Order CreateOrder(Order newOrder, string userId)
        {
            var cart = this.GetCart();

            newOrder.DateCreated = DateTime.Now;
           // newOrder.UserId = userId;

            this.Db.Order.Add(newOrder);

            if (newOrder.OrderItem == null)
                newOrder.OrderItem = new List<OrderItem>();

            decimal cartTotal = 0;

            foreach (var cartItem in cart)
            {
                var newOrderItem = new OrderItem()
                {
                    AlbumId = cartItem.Album.AlbumId,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.Album.Price
                };

                cartTotal += (cartItem.Quantity * cartItem.Album.Price);

                newOrder.OrderItem.Add(newOrderItem);
            }

            newOrder.TotalPrice = cartTotal;

            this.Db.SaveChanges();

            return newOrder;
        }
        public void EmptyCart()
        {
            session.Set<List<CartItem>>(CartSessionKey, null);
        }
    }
}