using Infrastructure.Views;

namespace ApplicationCore.Views
{
    public class TradeSessionViewModel : BaseEntityViewModel
    {
        public bool Default { get; set; }
        public int SymbolId { get; set; }
        public int Open { get; set; }
        public int Close { get; set; }
    }
}
