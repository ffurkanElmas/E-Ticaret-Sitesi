using Microsoft.AspNet.Identity.EntityFramework;
using DokumanModulu.Models;
using System.Data.Entity;

namespace DokumanModulu.Identity
{
    public class IdentityDataContext : IdentityDbContext<User>
    {
        public IdentityDataContext() : base("dataConnection")
        {
            Database.SetInitializer(new IdentityInitializer());
        }

        public static IdentityDataContext Create()
        {
            return new IdentityDataContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
