using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore
{
    public class SettingsKeys
    {
        public static string AppSettings = "AppSettings";
        public static string AuthSettings = "AuthSettings";
        public static string AdminSettings = "AdminSettings";
        public static string EcPaySettings = "EcPaySettings";
        public static string RootSubjectSettings = "RootSubjectSettings";
        public static string SubscribesSettings = "SubscribesSettings";
        public static string CloudStorageSettings = "CloudStorageSettings";
    }

    public class ApiKeyConstants
    {
        public const string HeaderName = "X-Api-Key";
    }
    public class SymbolCodes
    {
        public static string TXI = "TXI";
        public static string TXF = "TXF";
        public static string BTCUSD = "BTCUSD";
    }
    public enum HttpClients
    {
        Google
    }

    public enum AppRoles
    {
        Boss,
        Dev,
        Subscriber
    }


    public enum BrokageName
    {
        CONCORD = 0,
        HUA_NAN = 1,
        ONRICH = 2,
        CAPITAL = 3,
        BINANCE = 4,
        FAKE = 5
    }

    public enum ConnectionStatus
    {
        DISCONNECTED = 0,
        CONNECTING = 1,
        CONNECTED = 2
    }


}
