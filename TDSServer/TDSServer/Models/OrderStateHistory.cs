using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TDSServer.Models
{
    public class OrderStateHistory : BaseModel
    {
        public Order Order { get; set; }
        public OrderState OrderState { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
