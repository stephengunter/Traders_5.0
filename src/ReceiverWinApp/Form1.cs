using ApplicationCore;
using ApplicationCore.Managers;
using ApplicationCore.Receiver;
using ApplicationCore.Receiver.Services;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ApplicationCore.Helpers;

namespace ReceiverWinApp
{
    public partial class Form1 : Form
    {
        private static readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();
        private ISettingsManager _settingsManager;
        private ITimeManager _timeManager;
        private IQuoteSource _quoteSource;

        IFuturesLocalService _futuresLocalService;

        bool _closed = false;
        public Form1()
        {
            InitializeComponent();

            _settingsManager = Factories.CreateSettingsManager();

            string begin = _settingsManager.GetSettingValue(AppSettingsKey.Begin);
            string end = _settingsManager.GetSettingValue(AppSettingsKey.End);
            _timeManager = Factories.CreateTimeManager(begin, end);

            _futuresLocalService = Factories.CreateFuturesLocalService();

            InitReceiver();
        }

        IEnumerable<string> _symbolCodes = new List<string> { "MTX00" };

        void InitReceiver()
        {
            string name = _settingsManager.GetSettingValue(AppSettingsKey.QuoteSource);

            _quoteSource = Factories.CreateQuoteSource(name, _settingsManager.BrokageSettings);
            _quoteSource.ActionExecuted += Receiver_ActionExecuted;
            _quoteSource.ConnectionStatusChanged += Receiver_ConnectionStatusChanged;

            _quoteSource.NotifyStockTick += Source_NotifyStockTick;
            _quoteSource.NotifyFuturesTick += Source_NotifyFuturesTick;

        }

        private void Source_NotifyStockTick(object sender, EventArgs e)
        {
            var args = e as TickEventArgs;
            
        }

        private void Receiver_ConnectionStatusChanged(object sender, EventArgs e)
        {
            var args = e as ConnectionStatusEventArgs;
            if (args.Status == ConnectionStatus.CONNECTED)
            {
                _quoteSource.RequestQuotes(_symbolCodes);
            }
            else if (args.Status == ConnectionStatus.DISCONNECTED)
            { 
                if(!_closed) _quoteSource.Connect();
            }
        }
        
        private void Source_NotifyFuturesTick(object sender, EventArgs e)
        {
            var args = e as TickEventArgs;
            string code = args.Code;
            var tick = args.Tick;

            _futuresLocalService.SaveTick(tick);

            _logger.Info($"Code:{code}, Order:{tick.Order}, Time:{tick.Time}, Price: {tick.Price}");

        }

        private void Receiver_ActionExecuted(object sender, EventArgs e)
        {
            var args = e as ActionEventArgs;
            _logger.Info($"ActionExecuted: {args.Action} , Code: {args.Code} , Msg: {args.Msg}");
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            _quoteSource.Connect();
        }

        void SaveSymbols(Dictionary<short, List<string>> symbols)
        {
            foreach (var key in symbols.Keys)
            {
                string fileName = $"symbols_{key.ToString()}.txt";
                string filePath = System.IO.Path.Combine(_settingsManager.LogFilePath, fileName);
                var list = symbols[key];

                string content = "";
                foreach (var item in list)
                {
                    content += (item + Environment.NewLine);
                }
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine(content);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Dictionary<short, List<string>> symbols = _quoteSource.Test();
            //SaveSymbols(symbols);

            _quoteSource.DisConnect();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _closed = true;
            _quoteSource.DisConnect();
        }
    }
}
