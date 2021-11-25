using Microsoft.Azure.Documents.Client;
using OrderFunction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderFunction.Services
{
    public class DashboardService
    {
        //Name of the Cosmos Db databasae
        private static readonly string DatabaseID = "OrderDB";
        // Name of the container
        private static readonly string CollectionId = "Order";

        private static readonly Uri serviceEndPoint = new Uri(Environment.GetEnvironmentVariable("DatabaseEndpoint"));
        private static readonly DocumentClient client = new DocumentClient(serviceEndPoint, Environment.GetEnvironmentVariable("DatabaseAccountKey"));
        private static readonly Uri docuementCollectionLink = UriFactory.CreateDocumentCollectionUri(DatabaseID, CollectionId);

        public DashboardViewModel GetDashboardViewModel()
        {
            DashboardViewModel dashboardViewModel = new DashboardViewModel();
            try
            {
                var query = string.Format("SELECT * FROM c");
                var queryResult = client.CreateDocumentQuery<Order>(docuementCollectionLink, query, new FeedOptions { EnableCrossPartitionQuery = true })
                  .ToList();
                var completedOrders = queryResult.Where(a => a.OrderStatus == "Completed")
                         .GroupBy(a => a.OrderingProvince)
                         .Select(g => new { g.Key, Count = g.Count() });
                var draftOrders = queryResult.Where(a => a.OrderStatus == "Draft")
                         .GroupBy(a => a.OrderingProvince)
                         .Select(g => new { g.Key, Count = g.Count() });
                var cancelledOrders = queryResult.Where(a => a.OrderStatus == "Cancelled")
                         .GroupBy(a => a.OrderingProvince)
                         .Select(g => new { g.Key, Count = g.Count() });
                dashboardViewModel.CompletedOrdersByProvince = completedOrders.ToDictionary(x=>x.Key,x=>x.Count.ToString());
                dashboardViewModel.DraftOrdersByProvince = draftOrders.ToDictionary(x => x.Key, x => x.Count.ToString());
                dashboardViewModel.CancelledOrdersByProvince = cancelledOrders.ToDictionary(x => x.Key, x => x.Count.ToString());
            }
            catch(Exception)
            {
                dashboardViewModel = null;
            }
            return dashboardViewModel;
        }
    }
}
