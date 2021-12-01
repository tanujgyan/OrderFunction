using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using OrderFunction.Services;

namespace OrderFunction
{
    public static class OrderTriggerFunction
    {
        [FunctionName("OrderTriggerFunction")]
        public static async Task RunAsync([CosmosDBTrigger(
            databaseName: "OrderDB",
            collectionName: "Order",
            ConnectionStringSetting = "CosmosDbConnectionString",
            LeaseCollectionName = "leases", CreateLeaseCollectionIfNotExists =true)]IReadOnlyList<Document> input, ILogger log,
            [SignalR(HubName = "ordersdashboard")] IAsyncCollector<SignalRMessage> signalRMessages)
            {
            try
            {
                var viewModel = new DashboardService().GetDashboardViewModel();
                if (input != null && input.Count > 0)
                {
                    log.LogInformation("Documents modified " + input.Count);
                    log.LogInformation("First document Id " + input[0].Id);
                    await signalRMessages.AddAsync(new SignalRMessage
                    {
                        Target = "target",
                        Arguments = new[] { JsonSerializer.Serialize(viewModel) }
                    });
                }
               
            }
            catch(Exception ex)
            {
                log.LogError(ex.Message);
            }
        }
    }
}
