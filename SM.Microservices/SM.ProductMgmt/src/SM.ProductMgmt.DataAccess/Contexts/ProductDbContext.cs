using System.Reflection;
using SM.Core.DataAccess.Contexts;
using SM.Core.Utilities.Security.Jwt;
using DotNetCore.CAP;
using SM.Core.Services.EntityChangeServices;
using SM.Core.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SM.ProductMgmt.Domain.Concrete;

namespace SM.ProductMgmt.DataAccess.Contexts
{
    public class ProductDbContext : ProjectDbContext, IMsContext
    {
        public ProductDbContext(ITokenHelper tokenHelper, IConfiguration configuration, ICapPublisher capPublisher, IEntityChangeServices entityChangeServices) : base(tokenHelper,
            configuration, capPublisher, entityChangeServices)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("product");
            var asssebly = Assembly.GetExecutingAssembly();

            modelBuilder.ApplyConfigurationsFromAssembly(asssebly);
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string machineName = Environment.MachineName;
                base.OnConfiguring(optionsBuilder.UseNpgsql(_configuration.GetConnectionString("Postgres")));
            }
            //optionsBuilder.UseLowerCaseNamingConvention();
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("Postgres"), x => x.MigrationsHistoryTable("__EFMigrationsHistory", "product"));

            _options = optionsBuilder.Options;
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
