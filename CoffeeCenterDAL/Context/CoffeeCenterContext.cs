using System;
using System.Data.Entity;

namespace CoffeeCenterDAL.Context
{
    public class CoffeeCenterContext : DbContext
    {
        public DbSet<Table> Tables { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<OrderProducts> OrderProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public CoffeeCenterContext() : base("CoffeeCenter"){}
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                        .HasMany(or => or.OrderProducts)
                        .WithRequired(op => op.Order);

            modelBuilder.Entity<Product>()
                        .HasMany(p => p.OrderProducts)
                        .WithRequired(op => op.Product);

            modelBuilder.Entity<User>()
                        .HasMany(us => us.Reservations)
                        .WithOptional(r => r.User);
        }
    }
}
