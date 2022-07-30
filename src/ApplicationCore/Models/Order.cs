using System;
using System.Collections.Generic;
using Infrastructure.Entities;

namespace ApplicationCore.Models
{
    public class Order : BaseRecord
    {
        public string Symbol { get; set; }
        public OrderType Type { get; set; }
        public BS BS { get; set; }
        public decimal Price { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public decimal DealPrice { get; set; }
    }


    public enum OrderType
    {
        ROD = 0,
        MKT = 1,
        STP = 2
    }

    public enum BS
    {
        S = 0,
        B = 1
    }

}
