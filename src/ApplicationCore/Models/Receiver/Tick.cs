using Infrastructure.Entities;

namespace ApplicationCore.Models
{
    public class Tick : BaseEntity
    {
        public string Symbol { get; set; }       
        public int Date { get; set; }
        public int Time { get; set; }
        public int Order { get; set; }
        public decimal Price { get; set; }
        public decimal Bid { get; set; }
        public decimal Offer { get; set; }
        public decimal Qty { get; set; }
    }
}
