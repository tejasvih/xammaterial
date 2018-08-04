using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;
using Calibre.Models;
using Calibre.REST;
using Calibre.DataModels;
using Calibre.Xam.Log;
using static Calibre.Xam.Log.Logger;

namespace Calibre.Db
{
    
    public class dbService
    {
        
        public delegate void InitDelegate(dbService db);
        
        //static dbService _dbService;

        
        public LogDelegate OnLog { get; set; }

        static SQLiteAsyncConnection db;
        static SQLiteConnectionWithLock syncDb;
        public static SQLiteAsyncConnection Db
        {
            get
            {
                return db;
            }
        }
        public static SQLiteConnectionWithLock SyncDb
        {
            get
            {
                return syncDb;
            }
        }
        void log(LogType type, string message)
        {
            OnLog?.Invoke(this, type, message);
        }
        public dbService(string dbPath, InitDelegate InitAction, LogDelegate logDelegate = null)
        {
            OnLog = logDelegate;
            db = new SQLiteAsyncConnection(dbPath);
            syncDb = db.GetConnection();
            InitAction?.Invoke(this);
        }
        
        public void CreateTable<T>() where T : BaseModel, new()
        {
            try
            {
                var tableName = typeof(T).Name;
                var isTableExists = IsTableExists(tableName);
                if (!isTableExists)
                {
                    SyncDb.CreateTable<T>();
                    //dbTableService.Init<T>();
                }
                
            }
            catch (Exception ex) {
                Debug.WriteLine(ex);
                log(LogType.ERROR, ex.Message);
            }
        }

        public bool IsTableExists(string tableName)
        {
            var info = SyncDb.GetTableInfo(tableName);
            return info.Any();
        }

        /*async public void CreateTableAsync<T>() where T : BaseModel, new()
        {
            try
            {
                await db.CreateTableAsync<T>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                log(LogType.ERROR, ex.Message);
            }
        }*/

        
    }


}
