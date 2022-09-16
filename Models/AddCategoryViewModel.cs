using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Repository;
using WebShop.Validation;

namespace WebShop.Models
{
    public class AddCategoryViewModel
    {
        [DisplayName("Category Id")]
        public string CategoryId { get; set; }
        [DisplayName("Category Name")]
        public string CategoryName { get; set; }

    }
}