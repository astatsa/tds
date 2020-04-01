using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace TDSServer.Models
{
    public enum OrderStates
    {
        New,
        Viewed,
        Loaded,
        Completed
    }

    public class OrderState : BaseModel
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderStates Name { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public ICollection<Order> Orders { get; set; }
    }
}
