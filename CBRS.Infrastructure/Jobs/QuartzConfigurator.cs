using System;
using Microsoft.Extensions.Configuration;
using Quartz;

namespace CBRS.Infrastructure.Jobs
{
    public static class QuartzConfigurator
    {
        public static void AddJobAndTrigger<T>(
            this IServiceCollectionQuartzConfigurator quartz,
            IConfiguration config)
            where T : IJob
        {
            string jobName = typeof(T).Name;
            var key = $"Jobs:CBR";
            var cronSchedule = config[key];
            if (string.IsNullOrEmpty(cronSchedule))
                throw new Exception($"No Quartz.NET Cron schedule found for job in configuration at {key}");
            
            var jobKey = new JobKey(jobName);
            quartz.AddJob<T>(opts => 
                opts.WithIdentity(jobKey));

            quartz.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity(jobName + "-trigger")
                .WithCronSchedule(cronSchedule));
        }
    }
}