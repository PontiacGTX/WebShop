
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public AppDBContext ctx { get; set; }
        public OrderRepository(AppDBContext appCtx)
        {
            ctx = appCtx;
        }


        public async Task<bool?> Delete(Guid OrderId)
        {

            Order order = ctx.Orders.FirstOrDefault(x => x.OrderId == OrderId);
            if (order is null)
                return null;


            IEnumerable<OrderProducts> orderProd = ctx.OrderProducts.Where(x => x.OrderId == OrderId);
            ctx.OrderProducts.RemoveRange(orderProd);
            await ctx.SaveChangesAsync();
            ctx.Orders.Remove(order);
            return await ctx.SaveChangesAsync() > 0;




        }

        public async Task<IEnumerable<T>> GetCasted<T>(Func<Order, bool> selector = null, Func<OrderProducts, bool> selectorx = null)
        {
            return (await Get_In<T>(selector, selectorx)).Cast<T>();
        }
        async Task<IQueryable<object>> Get_In<T>(Func<Order, bool> selector = null, Func<OrderProducts, bool> selectorx = null)
         => typeof(T).Name switch
         {
             nameof(Order) => ctx.Orders.Where(x => selector(x)),
             nameof(OrderProducts) => ctx.OrderProducts.Where(x => selectorx(x)),
             _ => null
         };


        public async Task<IQueryable<IEntity>> Get<T>(Func<Order, bool> selector = null, Func<OrderProducts, bool> selectorx = null)
         => typeof(T).Name switch
            {
                nameof(Order) =>ctx.Orders.Where(x => selector(x)).Cast<IEntity>(),
                nameof(OrderProducts)=> ctx.OrderProducts.Where(x => selectorx(x)).Cast<IEntity>(),
                 _=> null,
            };

        public async Task<IEnumerable<T>> GetCasted<T>(Guid CustomerId) 
        {
            if(typeof(T) is Order)
            {
                return (await Get(CustomerId, ParamTEnum.ORDER)).Cast<T>();
            }
            else if(typeof(T) is OrderProducts)
            {
                return (await Get(CustomerId, ParamTEnum.ORDERPRODUCT)).Cast<T>();
            }
            return null;
         }
        public async Task<IEnumerable<object>> Get(Guid CustomerId, ParamTEnum paramT) =>
            paramT switch
            {
                ParamTEnum.ORDERPRODUCT=> await ctx.OrderProducts.Where(x => x.Order.Customer.CustomerId == CustomerId).ToListAsync(),
                ParamTEnum.ORDER => ctx.Orders.Where(x => x.Customer.CustomerId == CustomerId).ToList(),
                _=>null
            };
        public async Task<IEnumerable<Product>> GetOrderProducts(Guid OrderId) =>
        ctx.OrderProducts.Where(x => x.Order.OrderId == OrderId).Select(x=>x.Product).ToList();

        public async Task<Order> Get(Guid OrderId) =>
        await ctx.Orders.FirstOrDefaultAsync(x=>x.OrderId==OrderId);

        public async Task<Order> Add(Order order)
        {
            Guid empty = Guid.Empty;
            if(order.OrderId == empty)
            {
                Guid guid =  Guid.NewGuid();
                while(ctx.Orders.Any(x=>x.OrderId ==guid))
                {
                        guid = Guid.NewGuid();
                }
                order.OrderId = guid;
            }
            var res = await ctx.Orders.AddAsync(order);
            await ctx.SaveChangesAsync();
            return res.Entity;
        }





    }
}