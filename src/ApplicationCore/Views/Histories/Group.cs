using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Views
{
    public class QuoteGroupViewModel
    {
        public string Symbol { get; set; }
        public int Date { get; set; }
        public int Count { get; set; }
    }

    public class KLineGroupViewModel
    {
        public string Symbol { get; set; }
        public int Date { get; set; }
        public int Count { get; set; }
    }
}
