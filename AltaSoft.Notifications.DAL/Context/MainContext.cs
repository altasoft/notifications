using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltaSoft.Notifications.DAL.Context
{
    public class MainDbContext : DbContext
    {
        public MainDbContext()
            : base("name=DefaultConnectionString") { }

        public DbSet<Application> Applications { get; set; }
        public DbSet<ApplicationProduct> ApplicationProducts { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<SMS> SMS { get; set; }
    }
}
