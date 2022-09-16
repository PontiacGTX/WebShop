using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Repository
{
    public class OrderProductsRepository
    {
        AppDBContext ctx { get; }
        public OrderProductsRepository(AppDBContext appDBContext)
        {
            ctx = appDBContext;
        }

        public async Task<IEnumerable<OrderProducts>> GetOrderProducts()
           => ctx.OrderProducts;
        public async Task<IEnumerable<OrderProducts>> GetBy(Func<OrderProducts, bool> selector)
            => ctx.OrderProducts.Where(x => selector(x));
        public async Task<OrderProducts> AddOrdenProduct(OrderProducts orderProducts)
        {
            orderProducts.OrderProductId = Guid.NewGuid();
            while(ctx.OrderProducts.Any(x=>x.OrderProductId == orderProducts.OrderProductId))
            {
                orderProducts.OrderProductId = Guid.NewGuid();
            }

            EntityEntry<OrderProducts> entry = ctx.OrderProducts.Add(orderProducts);
            bool saved = await ctx.SaveChangesAsync()>0;

            return saved ? entry.Entity : null;
        }
        public async Task<OrderProducts> Update(OrderProducts orderProducts, Guid OrderProductsId)
        {
            OrderProducts orderProd = ctx.OrderProducts.FirstOrDefault(x => x.OrderProductId == OrderProductsId);
            if (orderProd is null)
                return null;
            bool isValid = ctx.Products.Any(x => x.ProductId == orderProducts.ProductId) && ctx.Orders.Any(x => x.OrderId == orderProducts.OrderId);
           
            if(!isValid)
                return null;
            
            orderProd.OrderId = orderProducts.OrderId;
            orderProd.ProductId = orderProducts.ProductId;
           bool saved = await ctx.SaveChangesAsync()>0;

            return saved ? orderProd : null;
        }
        public async Task<IEnumerable<OrderProducts>> DeleteWhere(Func<OrderProducts,bool> selector)
        {

            List<OrderProducts> orderProds = new List<OrderProducts>();
            IEnumerable<OrderProducts> orderProducts = ctx.OrderProducts.Where(x => selector(x));
            if (!orderProducts.Any())
                return orderProds;

            foreach (var ordrProdct in orderProducts)
            {
                try
                {
                   EntityEntry<OrderProducts> entry= ctx.OrderProducts.Remove(ordrProdct);
                    bool deleted = await ctx.SaveChangesAsync()>0;

                    if (deleted)
                        orderProds.Add(entry.Entity);
                }
                catch
                {

                }
            }

            return orderProds;
        }
        public async Task<bool?> Delete(OrderProducts orderProducts)
        {
            OrderProducts orderProd = ctx.OrderProducts.FirstOrDefault(x => x.OrderId == orderProducts.OrderId && x.ProductId == orderProducts.ProductId);
            if (orderProd is null)
                return null;

            ctx.OrderProducts.Remove(orderProd);
            bool saved = await ctx.SaveChangesAsync()>0;

            return saved as bool?;
        }

    }
}
