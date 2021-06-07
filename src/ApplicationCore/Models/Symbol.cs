using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class Symbol : BaseRecord
    {
        public SymbolType Type { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public TimeZone TimeZone { get; set; }

    }
    public enum SymbolType
    { 
        Stock,
        Index,
        Futures,
        Crypto
    }

    public enum TimeZone
    {
        TW,
        UTC
    }

}
