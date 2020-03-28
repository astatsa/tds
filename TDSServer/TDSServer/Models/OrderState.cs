using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public OrderStates Name { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
