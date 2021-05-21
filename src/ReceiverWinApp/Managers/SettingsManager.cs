using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using ApplicationCore.Receiver;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Security;
using ApplicationCore.Managers;
using ApplicationCore.Brokages;

namespace ReceiverWinApp
{
    public interface ISettingsManager
    {
        string GetSettingValue(string key);
        bool IsDevelopment { get; }
        BrokageSettings BrokageSettings { get; }
        string LogFilePath { get; }
        string SecurityKey { get; }

        bool CheckBasicSetting();
        string AddUpdateAppSettings(string key, string value);

    }

    public class SettingsManager : BaseSettingsManager, ISettingsManager
    {
        public SettingsManager()
        {
            _brokageSettings = new BrokageSettings
            {
                SID = GetSettingValue(AppSettingsKey.SID),
                Password = DecryptPassword(GetSettingValue(AppSettingsKey.Password)),
                LogFile = GetSettingValue(AppSettingsKey.LogFile)
            };
        }

        private BrokageSettings _brokageSettings;
        public BrokageSettings BrokageSettings => _brokageSettings;

        public string LogFilePath => GetSettingValue(AppSettingsKey.LogFile);

        public string SecurityKey => GetSettingValue(AppSettingsKey.SecurityKey);

        public bool CheckBasicSetting()
        {
            string sid = _brokageSettings.SID;
            if (String.IsNullOrEmpty(sid)) return false;

            string password = _brokageSettings.Password;
            if (String.IsNullOrEmpty(password)) return false;

            return true;
        }

        string DecryptPassword(string val)
        {
            try
            {
                string password = CryptoGraphy.DecryptCipherTextToPlainText(val, SecurityKey);
                return password;
            }
            catch (Exception)
            {
                return "";
            }
        }

    }
}
