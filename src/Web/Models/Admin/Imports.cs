using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class Import
    {
        [Index(0)]
        public int Date { get; set; }

        [Index(1)]
        public string Symbol { get; set; }

        [Index(2)]
        public string YearMonth { get; set; }

        [Index(3)]
        public int Time { get; set; }

        [Index(4)]
        public int Price { get; set; }

        [Index(5)]
        public int Qty { get; set; }
    }
}
