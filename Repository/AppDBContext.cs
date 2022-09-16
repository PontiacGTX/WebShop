using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebShop.Repository
{
    public class AppDBContext:IdentityDbContext<ApplicationUser>
    {
        public AppDBContext(DbContextOptions<AppDBContext>options):base(options)
        {
                
        }
    
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderProducts> OrderProducts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductCategories> ProductCategories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<OrderProducts>().HasKey(new string[] {nameof(Order.OrderId), nameof(Product.ProductId)});
           
            modelBuilder.Entity<ProductCategories>().HasKey(new string[] { nameof(Product.ProductId), nameof(Category.CategoryId) });
            base.OnModelCreating(modelBuilder);
        }
    }
}