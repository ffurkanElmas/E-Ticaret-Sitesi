using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DokumanModulu.Identity;

namespace DokumanModulu.Entity
{
    public class DataContext:DbContext
    {
        public DataContext() : base("dataConnection")
        {
            Database.SetInitializer(new DataInitializer());
        }

        public DbSet<DocumentTracking> Documents { get; set; }
    }
}