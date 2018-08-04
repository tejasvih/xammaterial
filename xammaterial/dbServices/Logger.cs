using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Calibre.Xam.Log
{
    public enum LogType
    {
        DONE, ERROR, INFO
    }
    public class Logger
    {
        public delegate void LogDelegate(object db, LogType type, string message);
        private static Object thisLock = new Object();
        /// <summary>
        /// called from threads, so use lock
        /// </summary>
        /// <param name="db"></param>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public static void Log(object db, LogType type, string message)
        {
            lock (thisLock)
            {
                //Log
                Debug.WriteLine(message);
            }
        }
    }
    
}
