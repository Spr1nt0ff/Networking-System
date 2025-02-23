using CountryLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DatabaseContext
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies().UseSqlServer(@"Server=SPR1NT\SQLEXPRESS;Database=CountriesDB;Integrated Security=SSPI;TrustServerCertificate=true");
        }
    }
}
