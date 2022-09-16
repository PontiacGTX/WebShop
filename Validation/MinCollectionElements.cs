using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Validation
{
    public class MinCollectionElementsAttribute:ValidationAttribute
    {
        public int MinimumElements { get; set; }
        public MinCollectionElementsAttribute()
        {
            MinimumElements = 1;
        }
        public override bool IsValid(object value)
        {
            return value is IList { Count: > 0 };
        }
        
    }
}
