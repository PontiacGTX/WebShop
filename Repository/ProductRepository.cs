using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Repository
{
    public class ProductRepository
    {
        AppDBContext ctx { get; }
        IWebHostEnvironment _WebHostEnvironment { get; }
        string GetProductFolder => System.IO.Directory.GetDirectories(_WebHostEnvironment.WebRootPath)?.FirstOrDefault(x => x.ToLower().Contains("products"));
        public ProductRepository(AppDBContext appDBContext, IWebHostEnvironment webHostEnvironment)
        {
            ctx = appDBContext;
            _WebHostEnvironment = webHostEnvironment;
        }

        public async Task<Product> GetProduct(Guid ProductId)
        => ctx.Products.FirstOrDefault(x => x.ProductId == ProductId);

        public async Task<List<string>> GetProductImages(Guid productId)
            => Directory.Exists(string.Concat(GetProductFolder, Path.AltDirectorySeparatorChar, productId))?
            Directory.GetFiles(string.Concat(GetProductFolder, Path.AltDirectorySeparatorChar, productId)).ToList(): new List<string>();

        public async Task<IEnumerable<Product>> GetProducts()
            => ctx.Products; 
        public async Task<IEnumerable<Category>> GetProductCategories(Guid ProductId)
        => from prodCat in ctx.ProductCategories.Where(x => x.Product.ProductId == ProductId)
            join cat in ctx.Categories on prodCat.Category.CategoryId equals cat.CategoryId
            where prodCat.Product.Enabled
            select cat;

        public async Task<IEnumerable<OrderProducts>> GetOrderProducts(Guid OrderId)
         => ctx.OrderProducts.Where(x => x.Order.OrderId == OrderId);

        public async Task<IEnumerable<Product>> GetProductsByCategory(Guid CategoryId)
            => ctx.ProductCategories.Where(x => x.Category.CategoryId == CategoryId).Select(X=>X.Product);
        public async Task<Product> AddProduct(Product product)
        {
            product.ProductId = Guid.NewGuid();
            while(ctx.Products.Any(x=>x.ProductId==product.ProductId))
            {
                product.ProductId = Guid.NewGuid();
            }

            EntityEntry<Product> p = ctx.Products.Add(product);
            bool saved = (await ctx.SaveChangesAsync()) > 0;

            return saved ? p.Entity :null;
        }
        public async Task<Product> UpdateProduct(Product product,Guid OldProductId)
        {
            Product oldProduct = await ctx.Products.FindAsync(OldProductId);
            bool saved = false;
            if(oldProduct is not null)
            {
                oldProduct.Price = product.Price;
                oldProduct.ProductName = product.ProductName;
                oldProduct.Quantity = product.Quantity;
                oldProduct.Enabled = product.Enabled;
                oldProduct.HasCategory = product.HasCategory;
                saved = await ctx.SaveChangesAsync()>0;
               
        
            }

            return saved ? await ctx.Products.FindAsync(OldProductId) : null;
        }

        
        public async Task<bool> DeleteProduct(Guid ProductId)
        {
            var prodCats = ctx.OrderProducts.Where(x => x.Product.ProductId == ProductId);
            bool updated = false;

            Product p = null;
            if (!prodCats.Any())
            {
                p =await  ctx.Products.FirstOrDefaultAsync(x => x.ProductId == ProductId);
                if (p is not null)
                {
                    ctx.ProductCategories.RemoveRange(ctx.ProductCategories.Where(x => x.Product.ProductId == ProductId));
                    ctx.Products.Remove(p);
                     updated = await ctx.SaveChangesAsync() > 0;
                    return updated;
                }
                
            }
            p = await ctx.Products.FirstOrDefaultAsync(x => x.ProductId == ProductId);
            if(p is not null)
            {
                p.Enabled = false;
                p.Quantity = 0;
                updated = await ctx.SaveChangesAsync() > 0;
            }
            
            return updated;
        }

        public async Task<bool> Exists(Guid ProductId) => 
            await ctx.Products.AnyAsync(x => x.ProductId == ProductId);


    }
}
