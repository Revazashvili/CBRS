using System.Diagnostics;
using System.Threading.Tasks;
using CBRS.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Quartz;

namespace CBRS.Core.Jobs
{
    [DisallowConcurrentExecution]
    public class RefreshJob : IJob
    {
        private readonly ILogger<RefreshJob> _logger;
        private readonly IRateService _rateService;

        public RefreshJob(ILogger<RefreshJob> logger,IRateService rateService)
        {
            _logger = logger;
            _rateService = rateService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            _logger.LogInformation("Start timer");
            await _rateService.RefreshData();
            stopwatch.Stop();
            _logger.LogInformation($"Time : {stopwatch.Elapsed.Seconds.ToString()}");
            _logger.LogInformation("stop timer");
        }
    }
}