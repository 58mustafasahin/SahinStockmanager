using Microsoft.EntityFrameworkCore;
using SM.AuthorizationMgmt.Business.Services.Authorizations;
using SM.AuthorizationMgmt.DataAccess.Abstract;
using SM.AuthorizationMgmt.DataAccess.Concrete.EntityFramework;
using SM.AuthorizationMgmt.DataAccess.Contexts;

namespace SM.AuthorizationMgmt.Api
{
    public static class Startup
    {
        public static void AddStartupRepository(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuthorizationDbContext>(options =>
            {
                options.UseNpgsql(configuration["ConnectionStrings:Postgres"], x =>
                {
                    x.MigrationsAssembly("SM.AuthorizationMgmt.DataAccess");
                });

                options.LogTo(Console.WriteLine, LogLevel.Information);
            });

            services.AddScoped<IUserRepository, UserRepository>();

        }

        public static void AddStartupServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
        }
    }
}
