using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderFunction.Models
{
    public class DashboardViewModel
    {
        [JsonProperty("CompletedOrdersByProvince")]
        public Dictionary<string,string> CompletedOrdersByProvince { get; set; }
        [JsonProperty("DraftOrdersByProvince")]
        public Dictionary<string, string> DraftOrdersByProvince { get; set; }
        [JsonProperty("CancelledOrdersByProvince")]
        public Dictionary<string, string> CancelledOrdersByProvince { get; set; }
    }
}
