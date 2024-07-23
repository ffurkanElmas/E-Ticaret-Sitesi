using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Staj.Entity;

namespace Staj.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public string Image { get; set; }
        public Category Category { get; set; }
    }
}