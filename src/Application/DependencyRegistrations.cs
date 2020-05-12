using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using WebsiteManagement.Application.Common;
using WebsiteManagement.Application.Common.Behaviours;
using WebsiteManagement.Application.Websites;
using WebsiteManagement.Application.Websites.Commands.CreateWebsite;

namespace WebsiteManagement.Application
{
    public static class DependencyRegistrations
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            // register all Fluent Validators
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // intercept all MediatR handlers with ValidationBehavior
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            // explicit registration validator and pipeline registrations
            // services.AddCreateWebsiteRegistrations();
        }

        private static void AddCreateWebsiteRegistrations(this IServiceCollection services)
        {
            services.AddSingleton<IValidator<CreateWebsite>, CreateWebsiteValidator>();
            services.AddScoped<IPipelineBehavior<CreateWebsite, OperationResult<WebsiteOutputModel>>, ValidationBehavior<CreateWebsite, OperationResult<WebsiteOutputModel>>>();
        }
    }
}
