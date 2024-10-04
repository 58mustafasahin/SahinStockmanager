using Microsoft.EntityFrameworkCore;
using SM.ProductMgmt.DataAccess.Abstract;
using SM.ProductMgmt.DataAccess.Concrete.EntityFramework;
using SM.ProductMgmt.DataAccess.Contexts;

namespace SM.ProductMgmt.Api
{
    public static class Startup
    {
        public static void AddStartupRepository(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ProductDbContext>(options =>
            {
                options.UseNpgsql(configuration["ConnectionStrings:Postgres"], x =>
                {
                    x.MigrationsAssembly("SM.ProductMgmt.DataAccess");
                });

                options.LogTo(Console.WriteLine, LogLevel.Information);
            });
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

        }

        public static void AddStartupServices(this IServiceCollection services)
        {

        }
    }
}
