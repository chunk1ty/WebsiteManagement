using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using WebsiteManagement.Application.Common.Behaviours;
using WebsiteManagement.Application.Interfaces;
using WebsiteManagement.Application.Websites.Commands.CreateWebsite;
using WebsiteManagement.Application.Websites.Commands.UpdateWebsite;
using WebsiteManagement.Application.Websites.Queries.GetWebsites;

namespace WebsiteManagement.Application
{
    public static class DependencyRegistrations
    {
        public static void AddApplication(this IServiceCollection services)
        {            
            services.AddMediatR(Assembly.GetExecutingAssembly());
            
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddSingleton<IValidator<CreateWebsite>, CreateWebsiteValidator>();
            services.AddSingleton<IValidator<UpdateWebsite>, UpdateWebsiteValidator>();
            services.AddSingleton<IValidator<GetWebsites>, GetWebsitesValidator>();
        }
    }
}
