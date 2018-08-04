using System;
using System.Collections.Generic;
using System.Text;

namespace Calibre
{

    public static class Constants
    {
        public const string ApiListingPrefix = "Get";
        public const string ApiListingSuffix = "Records";
        public const string ApiSavePrefix = "Save";
        public const string ApiSaveSuffix = "Record";
        public static string DBName = "main.db3";
        

        static Dictionary<string, object> urlInfo = new Dictionary<string, object>();
        
        public static string ServerAddress { get => urlInfo.ContainsKey("Host") ? (string)urlInfo["Host"] : ""; }
        public static int ServerPort { get => urlInfo.ContainsKey("Port") ? (int)urlInfo["Port"] : 80; }

        public static string Host {

            get {
                return (string)urlInfo["Host"];
            }
            set {
                urlInfo["Host"] = value;
            }
        }
        public static int Port
        {

            get
            {
                return (int)urlInfo["Port"];
            }
            set
            {
                urlInfo["Port"] = value;
            }
        }
        public static string ServerUrl
        {
            get
            {
                return $"{ServerAddress}{(ServerPort == 80 ? "" : (":" + ServerPort))}";
            }
        }
        public static string RestUrl
        {
            get { return $"http://{ServerUrl}/api/mobile/data"; }
        }
        public static string RestJsonUrl
        {
            get { return $"http://{ServerUrl}/api/mobile/json"; }
        }

        public static string RestUploadUrl
        {
            get { return $"http://{ServerUrl}/api/dal/upload"; }
        }
        
        public static string GetFileUrl
        {
            get { return $"http://{ServerUrl}/api/download/file"; }
        }
        public static string GetActionUrl(string controller,string action)
        {
            return $"http://{ServerUrl}/{controller}/{action}"; 
        }
        public const int RowHeight = 60;


    }
}
