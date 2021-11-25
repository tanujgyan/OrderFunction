using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using OrderFunction.Models;
using OrderFunction.Services;

namespace OrderFunction
{
    public static class DashboardLoadFunction
    {
        [FunctionName("DashboardLoadFunction")]
        public static DashboardViewModel Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var vm=  new DashboardService().GetDashboardViewModel();
            return vm;
        }
    }
}
