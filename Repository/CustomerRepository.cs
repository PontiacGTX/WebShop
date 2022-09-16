using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Repository
{
    public class CustomerRepository
    {
        AppDBContext ctx { get; set; }
        UserManager<ApplicationUser> _UserManager { get; }
        public CustomerRepository(UserManager<ApplicationUser> userManager,AppDBContext context)
        {
            ctx = context;
            _UserManager = userManager;

        }


        public async Task<ApplicationUser> GetUser(Guid CustomerId)
            => (await ctx.Customers.FirstOrDefaultAsync(x => x.CustomerId== CustomerId)).ApplicationUser;
        public async Task<Customer> GetCustomer(Guid CustomerId)
            => await ctx.Customers.FirstOrDefaultAsync(x => x.CustomerId == CustomerId);

        public async Task<Customer> AddCustomerId(Customer cus)
        {
            bool parsed =Guid.TryParse(cus.ApplicationUser.Id, out Guid currentGuid);
            cus.CustomerId = parsed ? currentGuid : Guid.NewGuid();
            while(ctx.Customers.Any(x=>x.CustomerId == cus.CustomerId))
            {
                cus.CustomerId = Guid.NewGuid();
            }
            EntityEntry<Customer> resCus =  ctx.Customers.Add(cus);
           var res =  await ctx.SaveChangesAsync();

            return res > 0 ? resCus.Entity : null; 
        }

        public async Task<bool?> DeleteCustomer(Guid CustomerGuid, string appUserId = null)
        {
            ApplicationUser user = await GetUser(CustomerGuid);
            if(user is null)
            {
                if(string.IsNullOrEmpty(appUserId))
                {
                    return null;
                }

                user = await _UserManager.FindByIdAsync(appUserId);
                if(user is null)
                {
                    return null;
                }
            }

            IdentityResult idRes = await  _UserManager.DeleteAsync(user);
            if (idRes.Succeeded)
            {
                Customer cus = await GetCustomer(CustomerGuid);
                ctx.Customers.Remove(cus);
               bool deleted= await ctx.SaveChangesAsync()>0;
                if(deleted)
                {
                    ctx.Customers.Remove(cus);
                    return (await ctx.SaveChangesAsync()) > 0 as bool?;
                }
            }
            

            return false;
        }

        public async Task<ApplicationUser> UpdateApplicationUser(ApplicationUser newData,Guid CustomerId)
        {

            IdentityResult res = null;
            Customer user = await ctx.Customers.FindAsync(CustomerId);
            if(user is null)
            {
                return null;
            }
          ApplicationUser appUser = await _UserManager.FindByIdAsync(user.ApplicationUser.Id);
            if (appUser is not null)
            {
                appUser.Address = newData.Address;
                appUser.UserName = newData.UserName;
                appUser.EmailConfirmed = newData.EmailConfirmed;
                appUser.Email = newData.Email;
                appUser.NormalizedEmail = newData.NormalizedEmail;
                appUser.PhoneNumber = newData.PhoneNumber;
                appUser.CustomerName = newData.CustomerName;
                res = await _UserManager.UpdateAsync(appUser);
                
            }

            return  res.Succeeded ? (await ctx.Customers.FindAsync(CustomerId)).ApplicationUser : null;

        }
    }
}
