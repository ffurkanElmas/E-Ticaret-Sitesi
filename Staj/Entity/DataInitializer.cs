using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Xml.Linq;

namespace Staj.Entity
{
    public class DataInitializer : DropCreateDatabaseIfModelChanges<DataContext>
    {
        protected override void Seed(DataContext context)
        {
            var kategoriler = new List<Category>()
            {
                new Category() { Name = "Erkek", Description = "Erkek Saatleri" },
                new Category() { Name = "Kadın", Description = "Kadın Saatleri" },
                new Category() { Name = "Unisex", Description = "Unisex Saatler" },
                new Category() { Name = "Çocuk", Description = "Çocuk Saatleri" },
            };

            foreach (var kategori in kategoriler) 
            { 
                context.Categories.Add(kategori);
            }

            context.SaveChanges();
            
            base.Seed(context);
        }
    }
}