using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SM.Core.DataAccess.Contexts;
using SM.Core.DataAccess;
using SM.Core.Utilities.Security.Jwt;
using System.Reflection;
using SM.AuthorizationMgmt.Domain.Concrete;
using DotNetCore.CAP;
using SM.Core.Services.EntityChangeServices;

namespace SM.AuthorizationMgmt.DataAccess.Contexts
{
    public class AuthorizationDbContext : ProjectDbContext, IMsContext
    {
        public AuthorizationDbContext(ITokenHelper tokenHelper, IConfiguration configuration, ICapPublisher capPublisher, IEntityChangeServices entityChangeServices) : base(tokenHelper,
            configuration, capPublisher, entityChangeServices)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("authorization");
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
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("Postgres"), x => x.MigrationsHistoryTable("__EFMigrationsHistory", "authorization"));

            _options = optionsBuilder.Options;
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
    }
}
