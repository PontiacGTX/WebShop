using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebShop.Repository
{
    
      public class ApplicationUser:IdentityUser
      {
          
        public string CustomerName { get; set; }
        public ulong CustomerIdentification { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Address { get; set; }
        public DateTime Birthday {get;set;}
      }

}