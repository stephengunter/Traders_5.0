using ApplicationCore.Exceptions;
using ApplicationCore.Helpers;
using SKCOMLib;
using System.Collections.Generic;

namespace ApplicationCore.Brokages.Binance
{
    public partial class BinanceBrokage
    {
        IEnumerable<string> _symbolCodes = new List<string>();

        private Dictionary<string, string> _symbolCodeBinanceCode = new Dictionary<string, string>
        {
            { SymbolCodes.BTCUSD, "BTCUSDT"}
        };

        private Dictionary<string, string> _binanceCodeSymbolCode = new Dictionary<string, string>();

        IEnumerable<string> BinanceSymbolCodes => _binanceCodeSymbolCode.Keys;
        void CheckSymbolCodes(IEnumerable<string> symbolCodes)
        {
            foreach (string code in symbolCodes)
            {
                if (!_symbolCodeBinanceCode.ContainsKey(code)) throw new SymbolCodeException($"錯誤的SymbolCode: {code}");
            }
        }

        void InitBinanceCodeSymbolCode(IEnumerable<string> symbolCodes)
        {
            
            CheckSymbolCodes(symbolCodes);

            _symbolCodes = symbolCodes;
            _binanceCodeSymbolCode = new Dictionary<string, string>();
            foreach (string code in symbolCodes)
            {
                string binanceCode = _symbolCodeBinanceCode[code];
                _binanceCodeSymbolCode.Add(binanceCode, code);
            }
        }
    }
}
