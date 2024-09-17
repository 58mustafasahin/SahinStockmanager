using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SM.Core.Common.Middleware;

namespace SM.AuthorizationMgmt.Business
{
    public static class BusinessStartup
    {
        public static IServiceCollection AddBusinessStartup(this IServiceCollection services)
        {
            services.AddMediatR(cf => cf.RegisterServicesFromAssemblies(typeof(BusinessStartup).Assembly));

            //Fluent validation settings

            //// After: Enabling auto-validation only
            services.AddFluentValidationAutoValidation();
            //// After: Enabling client validation only:
            services.AddFluentValidationClientsideAdapters();
            //services.AddValidatorsFromAssemblyContaining<LoginUserValidator>(ServiceLifetime.Transient);
            services.AddValidatorsFromAssembly(typeof(BusinessStartup).Assembly);
            services.AddControllers(option => option.Filters.Add<ValidationMiddleware>());

            //AddFluentValidation() [deprecated]
            //services.AddControllers(options =>
            //{
            //    options.Filters.Add<ValidationMiddleware>();
            //}).AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssembly(typeof(BusinessStartup).Assembly));

            //[ApiController] tag prevent default BadRequest return
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

            return services;
        }
    }
}
