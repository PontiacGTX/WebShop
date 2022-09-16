using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop.Repository
{
    public class CategoryRepository
    {
        AppDBContext ctx {  get; }
        public CategoryRepository(AppDBContext context)
        {
            ctx = context;
        }
        public async Task<Category> GetCategory(Guid CategoryId)
            => await ctx.Categories.FirstOrDefaultAsync(x => x.CategoryId == CategoryId);
        public async Task<IEnumerable<Category>> GetCategories()
            => await ctx.Categories.ToListAsync();
        public async Task<Category> UpdateCategory(Category category,Guid CategoryId)
        {
           Category cat =  ctx.Categories.FirstOrDefault(x => x.CategoryId == CategoryId);
            if (cat is null)
                return null;

            cat.CategoryName = category.CategoryName;
            bool saved = await ctx.SaveChangesAsync()>0;

            return saved ? cat:null;
        }

        public async Task<List<Guid>> AddProductCategories(List<CategorySelectModel>categories,Product p)
        {
            List<Guid> catIds = new List<Guid>();
            if (p.ProductId == Guid.Empty)
            {
                return null;
            }
            foreach (var categorySent in categories)
            {
                if (Guid.TryParse(categorySent.CategoryId, out Guid categoryId))
                {
                    try
                    {
                        if (ctx.Categories.Any(x => x.CategoryId == categoryId))
                        {
                            Guid mainIdx = Guid.NewGuid();
                            while (ctx.ProductCategories.Any(x => x.ProductCategoriesId == mainIdx))
                            {
                                mainIdx = Guid.NewGuid();
                            }

                            ctx.ProductCategories.Add(new ProductCategories { ProductCategoriesId = mainIdx, CategoryId = categoryId, ProductId = p.ProductId });
                            bool saved = await ctx.SaveChangesAsync() > 0;
                            if (saved)
                            {
                                catIds.Add(categoryId);
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            return catIds;
        }
        public async Task<bool?> DeleteCategory(Guid CategoryId)
        {
            bool? saved = false;
            IEnumerable<ProductCategories> prodCategories = ctx.ProductCategories.Where(x => x.Category.CategoryId == CategoryId);
            Category cat = await ctx.Categories.FindAsync(CategoryId);
            if (cat is null)
                return null;
            if (prodCategories.Any())
            {
                Product p = null;
                //var dic = prodCategories.ToDictionary(x=>x.Product.ProductId);
                foreach (var prodCat  in prodCategories)
                {
                    try
                    {
                        if(!ctx.ProductCategories.Any(x => x.Product.ProductId == prodCat.Product.ProductId && x.Category.CategoryId!=CategoryId))
                        {
                            p = ctx.Products.FirstOrDefault(x => x.ProductId == prodCat.Product.ProductId);
                            if (p is not null)
                            {
                                p.HasCategory = false;
                                await ctx.SaveChangesAsync();
                            }

                        }
                    }
                    catch (Exception)
                    {

                    }
                }
                ctx.ProductCategories.RemoveRange(prodCategories);
                saved  = await ctx.SaveChangesAsync()>0;
                ctx.Categories.Remove(cat);
                saved = await ctx.SaveChangesAsync() > 0;
            }
            return saved;
        }

        public  async Task<bool> Exist(Guid categoryId)
        {
            return await ctx.Categories.AnyAsync(x => x.CategoryId == categoryId);
        }

        public async Task<Category> AddCategory(Category category)
        {
            category.CategoryId = Guid.NewGuid();
            while(ctx.Categories.Any(x=>x.CategoryId==category.CategoryId))
            {
                category.CategoryId = Guid.NewGuid();

            }
            EntityEntry<Category> entry =  await ctx.Categories.AddAsync(category);
           bool saved= await ctx.SaveChangesAsync()>0;
            return saved ? entry.Entity:null;
        }


    }
}
