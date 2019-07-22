namespace SpodIglyMVC.DAL
{
    using SpodIglyMVC.Models;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class StoreContext : DbContext
    {

        public StoreContext()
            : base("name=StoreContext")
        {
        }

        static StoreContext()
        {
            Database.SetInitializer<StoreContext>(new StoreInitializer());
        }

        public DbSet<Genre> Genre { get; set; }

        public DbSet<Album> Album { get; set; }

        public DbSet<Order> Order { get; set; }

        public DbSet<OrderItem> OrderItem { get; set; } 
    }


}