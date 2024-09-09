using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SM.Core.Common.Configure;
using SM.Core.Services.EntityChangeServices;
using SM.Core.Utilities.Security.Jwt;
using System.Text;
using System.Text.Json.Serialization;

namespace SM.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ITokenHelper, JwtHelper>();
            services.AddScoped<IEntityChangeServices, EntityChangeServices>();

            var capConfig = configuration.GetSection("CapConfig").Get<CapConfig>();
            services.AddCap(options =>
            {
                options.UsePostgreSql(sqlOptions =>
                {
                    sqlOptions.ConnectionString = configuration.GetConnectionString("Postgres");
                    sqlOptions.Schema = "authorization";
                });
                options.UseRabbitMQ(rabbitMqOptions =>
                {
                    rabbitMqOptions.ConnectionFactoryOptions = connectionFactory =>
                    {
                        connectionFactory.Ssl.Enabled = capConfig.SslEnable;
                        connectionFactory.HostName = capConfig.HostName;
                        connectionFactory.UserName = capConfig.UserName;
                        connectionFactory.Password = capConfig.Password;
                        connectionFactory.Port = capConfig.Port;
                    };
                });
                options.UseDashboard(otp => { otp.PathMatch = "/MyCap"; });
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
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

