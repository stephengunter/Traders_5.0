using ApplicationCore;
using ApplicationCore.Managers;
using ApplicationCore.Receiver;
using NLog;
using ReceiverWinApp.Helpers;
using ReceiverWinApp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReceiverWinApp.Test
{
    public partial class BasicTestForm : Form
    {
        private static readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();
        private ISettingsManager _settingsManager;
        private ITimeManager _timeManager;
        private IQuoteSource _quoteSource;
        public BasicTestForm()
        {
            _settingsManager = Factories.CreateSettingsManager();

            this._timeManager = Factories.CreateTimeManager(_settingsManager.GetSettingValue(AppSettingsKey.Begin),
                _settingsManager.GetSettingValue(AppSettingsKey.End));

            InitializeComponent();

            CheckBasicSettings();
            if (_basicSettingOK)
            {
               //InitReceiver();
            }
            else
            {
                OnEditConfig(null, null);
            }

            InitBasicUI();
            InitStatusUI();
        }

        void InitReceiver()
        {
            string name = _settingsManager.GetSettingValue(AppSettingsKey.QuoteSource);

            _quoteSource = Factories.CreateQuoteSource(name, _settingsManager.BrokageSettings);
            _quoteSource.ActionExecuted += QuoteSource_ActionExecuted;
            _quoteSource.Ready += OnReceiverReady;
            _quoteSource.ConnectionStatusChanged += Receiver_ConnectionStatusChanged;

        }

        bool _closed = false;

        #region  Helper
        bool _basicSettingOK = false;
        bool CheckBasicSettings() => _basicSettingOK = _settingsManager.CheckBasicSetting();
        #endregion

        #region  UI
        UcStatus ucStatus;
        #endregion

        #region Form Event Handlers
        private void BasicTestForm_Load(object sender, EventArgs e)
        {
            //if (_quoteSource != null)
            //{
            //    if (_quoteSource.Connectted) this.OnReceiverReady(null, null);
            //    else _quoteSource.Connect();
            //}
            
        }
        #endregion

        #region Event Handlers

        private void QuoteSource_ActionExecuted(object sender, EventArgs e)
        {
            var args = e as ActionEventArgs;
            _logger.Info($"{args.Action} - {args.Code} - {args.Msg}");
        }
        private void OnReceiverReady(object sender, EventArgs e)
        {
            _logger.Info($"ReceiverReady. Provider: {_quoteSource.Name}");
        }

        private void OnEditConfig(object sender, EventArgs e)
        {
            this.editConfig = new EditConfig(_settingsManager, _timeManager);
            this.editConfig.ConfigChanged += this.OnConfig_Changed;

            this.editConfig.ShowDialog();
        }




        private void OnConfig_Changed(object sender, EventArgs e = null) => OnSettinsChanged();

        private void btnLogs_Click(object sender, EventArgs e)
        {
            var psi = new ProcessStartInfo() { FileName = _settingsManager.LogFilePath, UseShellExecute = true };
            Process.Start(psi);
        }

        private void Receiver_ConnectionStatusChanged(object sender, EventArgs e)
        {
            if (_closed) return;

            try
            {

                if (ucStatus == null) return;
                var args = e as ConnectionStatusEventArgs;
                if (args.Status != ConnectionStatus.CONNECTING) ucStatus.CheckConnect();

            }
            catch (Exception ex)
            {
                _logger.Error(ex);

            }
        }


        #endregion

        void InitBasicUI()
        {

            if (_basicSettingOK) this.tpTop.Controls.Add(UIHelpers.CreateLabel("基本設定", Color.Black, DockStyle.Fill), 0, 0);
            else this.tpTop.Controls.Add(UIHelpers.CreateLabel("您還沒有完成基本設定", Color.Red, DockStyle.Fill), 0, 0);

        }
        void InitStatusUI()
        {
            this.ucStatus = new UcStatus(_settingsManager, _timeManager, _quoteSource, _logger);
            this.panel1.Controls.Add(this.ucStatus);
        }


        void OnSettinsChanged()
        {
            MessageBox.Show("設定檔已變更, 程式將重新啟動.");
            Application.ExitThread();
            ReStart();
        }

        void ReStart()
        {
            Thread thtmp = new Thread(new ParameterizedThreadStart(Run));
            object appName = Application.ExecutablePath;
            Thread.Sleep(3000);
            thtmp.Start(appName);

        }

        private void Run(Object obj)
        {
            Process ps = new Process();
            ps.StartInfo.FileName = obj.ToString();
            ps.Start();

        }

        private void BasicTestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _closed = true;

            if (_quoteSource != null) _quoteSource.DisConnect();

            Thread.Sleep(1500);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (_quoteSource != null) _quoteSource.DisConnect();
        }


    }
}
