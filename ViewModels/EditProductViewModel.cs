using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Repository;

namespace WebShop.ViewModels
{
    public class EditProductViewModel
    {
       //[JsonPropertyName("productId")]
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public List<Category> Categories { get; set; } = new List<Category>();
    }
}
