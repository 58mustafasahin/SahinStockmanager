using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SM.Core.DataAccess.Contexts;
using SM.Core.Utilities.Security.Jwt;
using System.Text;

namespace SM.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IProjectContext, ProjectDbContext>();
            services.AddTransient<ITokenHelper, JwtHelper>();
        }

        public static void AddMediator(this IServiceCollection services, Type assemblyType)
        {
            services.AddMediatR(assemblyType.Assembly);
        }

        //public static IServiceCollection AddDistributedCacheProvider(this IServiceCollection services, IConfiguration configuration)
        //{
        //    var connectionString = configuration["Caching:Redis:ConnectionString"];
        //    ArgumentNullException.ThrowIfNull(connectionString);
        //    var instanceName = configuration["Caching:Redis:InstanceName"];
        //    ArgumentNullException.ThrowIfNull(instanceName);

        //    services.AddStackExchangeRedisCache(opts => { opts.Configuration = connectionString; opts.InstanceName = instanceName; });
        //    services.AddSingleton<IDistributedCacheProvider, DistributedCacheProvider>();
        //    services.AddSingleton<ICacheProvider>(sp => sp.GetRequiredService<IDistributedCacheProvider>());
        //    return services;
        //}

        //public static IServiceCollection AddMemoryCacheProvider(this IServiceCollection services)
        //{
        //    services.AddMemoryCache();
        //    services.AddSingleton<IMemoryCacheProvider, MemoryCacheProvider>();
        //    services.AddSingleton<ICacheProvider>(sp => sp.GetRequiredService<IMemoryCacheProvider>());
        //    return services;
        //}

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var tokenOptions = configuration.GetSection("TokenOptions").Get<TokenOptions>();
            services.Configure<TokenOptions>(configuration.GetSection("TokenOptions"));

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(jwt =>
           {
               jwt.SaveToken = true;
               jwt.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenOptions.SecurityKey)),
                   ValidateIssuer = true,
                   ValidIssuer = tokenOptions.Issuer,
                   ValidateAudience = true,
                   ValidAudience = tokenOptions.Audience,
                   ValidateLifetime = true,
                   ClockSkew = TimeSpan.Zero
               };
               jwt.Events = new JwtBearerEvents
               {
                   OnMessageReceived = context =>
                   {
                       // Authorization Header'ı varsa JWT standart olarak oradan ilerlesin.
                       if (context?.HttpContext?.Request?.Headers?.ContainsKey("Authorization") == true)
                       {
                           return Task.CompletedTask;
                       }

                       string jwtCookieTokenName = (string)configuration.GetSection("Cookie").GetValue(typeof(string), "Name");

                       if (!String.IsNullOrEmpty(jwtCookieTokenName))
                       {
                           string jwtCookieTokenValue = context.Request.Cookies[jwtCookieTokenName];

                           if (!String.IsNullOrEmpty(jwtCookieTokenValue))
                           {
                               context.Token = jwtCookieTokenValue;
                           }
                       }
                       return Task.CompletedTask;
                   }
               };
           });
            return services;
        }
    }
}

