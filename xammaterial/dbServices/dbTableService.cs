using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Threading.Tasks;
using Calibre.Models;
using System.Diagnostics;
using System.Linq;
using Calibre.REST;
using Calibre.DataModels;

namespace Calibre.Db
{
    /// <summary>
    /// Sync Methods on SQLite table operations
    /// </summary>
    public partial class dbTableService
    {

        public dbTableService()
        {

        }

        //Load
        public static T GetItem<T>(string gid) where T : BaseModel, new()
        {
            return dbService.SyncDb.Get<T>(gid);
        }

        public static T GetItem<T>(Func<T, bool> conditon) where T : BaseModel, new()
        {
            //return dbService.SyncDb.Get<T>(conditon);
            var item = dbService.SyncDb.Table<T>().ToList().Where(x => conditon(x)).FirstOrDefault();
            return item;
        }


        //Find
        public static bool Exists<T>(string gid) where T : BaseModel, new()
        {
            //return dbService.SyncDb.Find<T>(gid) != null;
            var s = gid.ToLower();
            return dbService.SyncDb.Find<T>( x =>
            x.Gid.ToLower()  == s
            ) != null;
            //return dbService.SyncDb.Table<T>().Count(x => x.Gid == gid) > 0;
        }

        //List
        public static List<T> List<T>() where T : /*BaseModel,*/ new()
        {
            return dbService.SyncDb.Table<T>().ToList();
        }

        public static List<T> List<T>(System.Linq.Expressions.Expression<Func<T, bool>> conditon) where T : BaseModel, new()
        {
            return dbService.SyncDb.Table<T>().Where(conditon).ToList();
        }
        public static List<T> List<T>(string sql, params object[] args) where T : BaseModel, new()
        {
            return dbService.SyncDb.Query<T>(sql, args).ToList();
        }

        //Save
        public static int Insert<T>(T item) where T : BaseModel, new()
        {
            return dbService.SyncDb.Insert(item);
        }
        public static int InsertAll<T>(IEnumerable<T> items) //where T : IEnumerable<BaseModel>, new()
        {
            return dbService.SyncDb.InsertAll(items);
        }
        public static int Update<T>(T item) where T : BaseModel, new()
        {
            return dbService.SyncDb.Update(item);
        }
        public static int UpdateAll<T>(T items) where T : IEnumerable<BaseModel>, new()
        {

            return dbService.SyncDb.UpdateAll(items);
        }
        public static int InsertOrUpdate<T>(T item) where T : BaseModel, new()
        {
            bool isRecordExists = !dbTableService.Exists<T>(item.Gid);
            if (isRecordExists)
            {
                return dbService.SyncDb.Insert(item);
            }
            else
                return dbService.SyncDb.Update(item);
        }

        //Delete
        public static int Delete<T>(T item) //where T : BaseModel, new()
        {
            return dbService.SyncDb.Delete(item);
        }

        /// <summary>
        /// Empties the table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static int DeleteAll<T>() //where T : BaseModel, new()
        {

            return dbService.SyncDb.DeleteAll<T>();
        }



    }


}
