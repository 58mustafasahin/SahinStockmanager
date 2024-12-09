using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SM.Core.DataAccess;
using SM.Core.DataAccess.Contexts;
using SM.Core.Services.EntityChangeServices;
using SM.Core.Utilities.Security.Jwt;
using SM.WarehouseMgmt.Domain.Concrete;
using System.Reflection;

namespace SM.WarehouseMgmt.DataAccess.Contexts
{
    public class WarehouseDbContext : ProjectDbContext, IMsContext
    {
        public WarehouseDbContext(ITokenHelper tokenHelper, IConfiguration configuration, ICapPublisher capPublisher, IEntityChangeServices entityChangeServices) : base(tokenHelper, configuration, capPublisher, entityChangeServices)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("warehouse");
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
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("Postgres"), x => x.MigrationsHistoryTable("__EFMigrationsHistory", "warehouse"));

            _options = optionsBuilder.Options;
        }
        
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<StockMovement> StockMovements { get; set; }
    }
}
