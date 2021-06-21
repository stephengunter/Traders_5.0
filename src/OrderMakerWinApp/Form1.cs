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
        private IOrderMaker _orderMaker;
        public Form1()
        {
            _settingsManager = Factories.CreateSettingsManager();

            string name = _settingsManager.GetSettingValue(AppSettingsKey.OrderMaker);

            _orderMaker = Factories.CreateOrderMaker(name, _settingsManager.BrokageSettings);
            _orderMaker.Ready += this.OnOrderMakerReady;
            _orderMaker.ActionExecuted += this.OnActionExcuted;
            if (_orderMaker.Name == BrokageName.HUA_NAN)
            {
                _orderMaker.AccountPositionUpdated += OrderMaker_AccountPositionUpdated;
            }

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (_orderMaker.Connectted) this.OnOrderMakerReady(null, null);
            else _orderMaker.Connect();
        }

        private void OnOrderMakerReady(object sender, EventArgs e)
        {
            _logger.Info($"OrderMakerReady. Provider: {_orderMaker.Name}");
        }

        private void OrderMaker_AccountPositionUpdated(object sender, EventArgs e)
        {
            //try
            //{
            //    var args = e as AccountEventArgs;
            //    string accountId = args.Account;

            //    if (Account.Number.EqualTo(args.Account))
            //    {
            //        Account.OI = _orderMaker.GetAccountPositions(Account.Number, Account.Symbol);

            //        Render();
            //    }

            //}
            //catch (Exception ex)
            //{
            //    _logger.Error(ex);

            //}
        }

        private void OnActionExcuted(object sender, EventArgs e)
        {
            try
            {
                var args = e as ActionEventArgs;
                _logger.Info($"ActionExcuted: {args.Action} , Code: {args.Code} , Msg: {args.Msg}");

            }
            catch (Exception ex)
            {
                _logger.Error(ex);

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _orderMaker.DisConnect();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _orderMaker.Connect();
        }
    }
}
