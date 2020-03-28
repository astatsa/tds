using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileApp.Models
{
    enum OrderStates
    {
        New,
        Viewed,
        Loaded,
        Completed
    }

    class OrderState : BaseModel
    {
        public OrderStates Name { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
    }
}
