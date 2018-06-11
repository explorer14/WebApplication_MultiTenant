using DataAcess.DomainModel;
using Microsoft.EntityFrameworkCore;

namespace DataAcess
{
    public class CRMContext : DbContext
    {
        private DbContextOptions contextOptions;

        public CRMContext()
            : base()
        {
        }

        public CRMContext(DbContextOptions options)
            : base(options)
        {
            this.contextOptions = options;
        }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasKey(e => e.CustomerId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string[] tenants = new string[] { "TenantA", "TenantB" };
                string connectionString = "Server=(localdb)\\mssqllocaldb;Database={tenant};Trusted_Connection=True;MultipleActiveResultSets=true";
                optionsBuilder.UseSqlServer(connectionString.Replace("{tenant}", "TenantA"));

                //foreach (string tenant in tenants)
                //{
                //    optionsBuilder.UseSqlServer(connectionString.Replace("{tenant}", "TenantA"));
                //}
            }
        }
    }
}