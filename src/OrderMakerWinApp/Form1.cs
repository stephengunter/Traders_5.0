using ApplicationCore;
using ApplicationCore.DtoMapper;
using ApplicationCore.Managers;
using ApplicationCore.OrderMaker;
using ApplicationCore.OrderMaker.Models;
using ApplicationCore.OrderMaker.ViewServices;
using AutoMapper;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrderMakerWinApp
{
    public partial class Form1 : Form
    {
        private static readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();
        private ISettingsManager _settingsManager;
        private ITimeManager _timeManager;
        private IOrderMaker _orderMaker;

        bool _closed = false;
        public Form1()
        {
            InitializeComponent();

            _settingsManager = Factories.CreateSettingsManager();

            InitOrderMaker();
        }

        void InitOrderMaker()
        {
            string name = _settingsManager.GetSettingValue(AppSettingsKey.OrderMaker);

            _orderMaker = Factories.CreateOrderMaker(name, _settingsManager.BrokageSettings);
            _orderMaker.ActionExecuted += OrderMaker_ActionExecuted;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _orderMaker.Connect();
        }

        private void OrderMaker_ActionExecuted(object sender, EventArgs e)
        {
            var args = e as ActionEventArgs;
            _logger.Info($"ActionExecuted: {args.Action} , Code: {args.Code} , Msg: {args.Msg}");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var accounts = _orderMaker.Test();
            var account = accounts.FirstOrDefault();

            string symbol = "TXO16000R1";
            decimal price = 3;
            int lots = 1;
            bool dayTrade = false;

            _orderMaker.MakeOrder(symbol, account, price, lots, dayTrade);




            //var psi = new System.Diagnostics.ProcessStartInfo() { FileName = _settingsManager.LogFilePath, UseShellExecute = true };
            //System.Diagnostics.Process.Start(psi);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _closed = true;
            _orderMaker.DisConnect();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _orderMaker.Connect();
        }
    }
}
