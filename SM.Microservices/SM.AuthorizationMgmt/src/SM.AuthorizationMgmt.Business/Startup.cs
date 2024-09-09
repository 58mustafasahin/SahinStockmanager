using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace SM.AuthorizationMgmt.Business
{
    public static class BusinessStartup
    {
        public static IServiceCollection AddBusinessStartup(this IServiceCollection services)
        {
            services.AddMediatR(cf => cf.RegisterServicesFromAssemblies(typeof(BusinessStartup).Assembly));
            return services;
        }
    }
}
