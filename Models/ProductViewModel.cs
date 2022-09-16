using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Repository;

namespace WebShop.Models
{
    public class ProductViewModel
    {
        public Product Product { get; set; } = new Product();
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
        public IList<IFormFile> PhotoPath { get; set; } = new List<IFormFile>();
    }
}
