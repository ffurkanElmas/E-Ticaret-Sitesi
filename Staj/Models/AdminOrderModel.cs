using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Staj.Entity;

namespace Staj.Models
{
    public class AdminOrderModel
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public int Count { get; set; }
        public double Total { get; set; }
        public DateTime OrderDate { get; set; }
        public EnumOrderStatus OrderStatus { get; set; }
        
    }
}