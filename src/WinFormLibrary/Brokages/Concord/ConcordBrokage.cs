using ApplicationCore.Exceptions;
using ApplicationCore.Helpers;
using ApplicationCore.Managers;
using ApplicationCore.OrderMaker;
using ApplicationCore.OrderMaker.Views;
using ApplicationCore.Receiver;
using ApplicationCore.Receiver.Views;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using Concord.API.Future.Client;
using ApplicationCore.Brokages;
using ApplicationCore;

namespace WinFormLibrary.Brokages.Concord
{
    public partial class ConcordBrokage : BaseOrderMaker
    {
        private ucClient _API = new ucClient();

        #region Consts
        private const string LOGIN_SUCCESS = "102";  //登入OK
        private const string ACCESS_FAIL = "201"; //登入主機無法連線
        private const string NOT_LOGIN = "202";  //尚未登入
        private const string CONNECTION_LOST = "210";  //下單連線中斷
        private const string LOGIN_FAILED = "211";  //下單登入驗證失敗
        #endregion
        
        public ConcordBrokage(BrokageSettings settings) : base(BrokageName.CONCORD, settings)
        {
            _API.OnFGeneralReport += new ucClient.dlgFGeneralReport(API_OnFGeneralReport);
            _API.OnFErrorReport += new ucClient.dlgFErrorReport(API_OnFErrorReport);
            _API.OnFOrderReport += new ucClient.dlgFOrderReport(API_OnFOrderReport);
        }
        string SID => BrokageSettings.SID.ToUpper();
        string Password => BrokageSettings.Password;
        string IP => BrokageSettings.IP;
        public override bool Connectted
        {
            get
            {
                string code = _API.FCheckConnect(out string msg);
                OnActionExecuted("CheckConnect", code, msg);
                return code == LOGIN_SUCCESS;
            }
        }
        public override void Connect() => Login();

        public override void DisConnect() => Logout();

        void Login()
        {
            SetConnectionStatus(ConnectionStatus.CONNECTING);

            string code = _API.Login(SID, Password, IP, out string msg);

            if (code == LOGIN_SUCCESS)
            {
                SetConnectionStatus(ConnectionStatus.CONNECTED);
                SetReady(true);
            }

            OnActionExecuted("Login", code, msg);

        }

        void Logout()
        {
            string code = _API.Logout(out string msg);
            OnActionExecuted("Logout", code, msg);
        }


        private void API_OnFGeneralReport(string strMsgCode, string strMsg)
        {
            OnActionExecuted(new ActionEventArgs("FGeneralReport", strMsgCode, strMsg));
        }
        private void API_OnFErrorReport(string strMsgCode, string strMsg)
        {
            OnActionExecuted(new ActionEventArgs("FErrorReport", strMsgCode, strMsg));
        }
        private void API_OnFOrderReport(string strMsgCode, string strMsg)
        {
            OnActionExecuted(new ActionEventArgs("FOrderRepor", strMsgCode, strMsg));
        }
        void OnActionExecuted(string action, string code, string msg)
        {
            if (code == ACCESS_FAIL || code == NOT_LOGIN) // 201 登入主機無法連線 202 尚未登入
            {
                SetConnectionStatus(ConnectionStatus.DISCONNECTED);
            }
            else if (code == CONNECTION_LOST || code == LOGIN_FAILED) // 下單連線中斷 , 登入驗證失敗
            {
                SetConnectionStatus(ConnectionStatus.DISCONNECTED);
            }

            OnActionExecuted(new ActionEventArgs(action, code, msg));
        }
    }
}
