using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Calibre;
using Calibre.Db;
using SQLite;
using xammaterial.Db;

namespace xammaterial
{
    public class AppInstance
    {
        public static AppInstance Instance;
        AppDb appDb;
        //static BaseModelSyncService syncService;
        static dbService dbs;
        //static SQLiteAsyncConnection mastersDb;
        //static SQLiteConnectionWithLock mastersSyncDb;


        /*public static SQLiteAsyncConnection MastersDb
        {
            get
            {
                return mastersDb;
            }
        }
        public static SQLiteConnectionWithLock MastersSyncDb
        {
            get
            {
                return mastersSyncDb;
            }
        }*/
        public static void Init()
        {
            if (Instance == null)
            {
                Instance = new AppInstance();
                Instance.Initialize();
            }
        }
        void Initialize()
        {
#if DEBUG

            Constants.Host = "192.168.1.81";
            Constants.Port = 59764;
#else
            Constants.Host = "panitbox.com";
            Constants.Port = 80;
#endif
            appDb = new AppDb();
            Constants.DBName = "AppDb" + Settings.LoggedInUserId + ".db3";
            dbs = new dbService(GetLocalFilePath(Constants.DBName), appDb.InitTables, appDb.dbLog);


            //string mastersDbPath = GetLocalFilePath("masters.db3");
            //mastersDb = new SQLiteAsyncConnection(mastersDbPath);
            //mastersSyncDb = mastersDb.GetConnection();
        }
        public string GetLocalFilePath(string filename)
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
        public string GetFolderPath(System.Environment.SpecialFolder kind = System.Environment.SpecialFolder.Personal)
        {

            return System.Environment.GetFolderPath(kind);

        }
        public string GetAbsolutePath()
        {
            var basePath = Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath;
            return basePath;

        }
        public string GetDownloadsPath()
        {
            var basePath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
            return basePath;
        }
        public string PersonalFolderPath { get => GetFolderPath(); }
        
        public string AbsolutePath { get => GetAbsolutePath(); }

        

        public string AppVersion
        {
            get
            {
                var context = Android.App.Application.Context;
                var info = context.PackageManager.GetPackageInfo(context.PackageName, 0);

                return $"{info.VersionName}.{info.VersionCode.ToString()}";
            }
        }
       
       
    }
}