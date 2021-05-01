using CBRS.Core.Jobs;
using CBRS.Core.Repositories;
using CBRS.Core.Services.Implementations;
using CBRS.Core.Services.Interfaces;
using CBRS.Infrastructure.Jobs;
using CBRS.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace CBRS.WorkerService.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCbrsModules(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddQuartzJobs(configuration)
                .AddRepositories()
                .AddServices();
            return services;
        }
        private static IServiceCollection AddQuartzJobs(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionScopedJobFactory();
                q.AddJobAndTrigger<RefreshJob>(configuration);
            });
            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            return services;
        }
        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IXmlService, XmlService>();
            services.AddScoped<IRateService,RateService>();
            return services;
        }
        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRateRepository, RateRepository>();
            return services;
        }
    }
}