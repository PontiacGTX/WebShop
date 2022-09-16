using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Repository
{
    public class ProductCategories
    {
        [Key]
        public Guid ProductCategoriesId { get; set; }
        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public Guid CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

    }
}
