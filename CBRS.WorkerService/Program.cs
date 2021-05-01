using CBRS.WorkerService.Extensions;
using Microsoft.Extensions.Hosting;
using NLog.Web;

namespace CBRS.WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseNLog()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddCbrsModules(hostContext.Configuration);
                });
    }
}