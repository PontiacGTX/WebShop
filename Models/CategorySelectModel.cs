using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Repository;

namespace WebShop.Models
{
    public class CategorySelectModel
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        //public long Index { get; set; }   
    }
}
