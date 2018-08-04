using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Threading.Tasks;


using Calibre.Models;

using System.Diagnostics;
using System.Linq;
using Calibre.DataModels;

namespace Calibre.Db
{
    /// <summary>
    /// Async Methods on SQLite table operations
    /// </summary>
    public partial class dbTableService
    {

        //Load
        async public static Task<T> GetItemAsync<T>(string gid) where T : BaseModel, new()
        {
            return await dbService.Db.GetAsync<T>(gid);
        }

        async public static Task<T> GetItemAsync<T>(Func<T, bool> conditon) where T : BaseModel, new()
        {
            return await dbService.Db.GetAsync<T>(conditon);
            //var item = dbService.SyncDb.Table<T>().ToList().Where(x => conditon(x)).FirstOrDefault();
            //return item;
        }


        //Find
        async public static Task<bool> ExistsAsync<T>(string gid) where T : BaseModel, new()
        {
            return await dbService.Db.FindAsync<T>(gid) != null;
            //return dbService.SyncDb.Table<T>().Count(x => x.Gid == gid) > 0;
        }

        //List
        async public static Task<List<T>> ListAsync<T>() where T : BaseModel, new()
        {
            return await dbService.Db.Table<T>().ToListAsync();
        }

        async public static Task<List<T>> ListAsync<T>(System.Linq.Expressions.Expression<Func<T, bool>> conditon) where T : BaseModel, new()
        {
            return await dbService.Db.Table<T>().Where(conditon).ToListAsync();
        }
        async public static Task<List<T>> ListAsync<T>(string sql, params object[] args) where T : new()
        {
            return await dbService.Db.QueryAsync<T>(sql, args);
        }

        //Save
        async public static Task<int> InsertAsync<T>(T item) where T : BaseModel, new()
        {
            return await dbService.Db.InsertAsync(item);
        }
        async public static Task<int> InsertAllAsync<T>(T items) where T : IEnumerable<BaseModel>, new()
        {
            return await dbService.Db.InsertAllAsync(items);
        }
        async public static Task<int> UpdateAsync<T>(T item) where T : BaseModel, new()
        {
            return await dbService.Db.UpdateAsync(item);
        }
        async public static Task<int> UpdateAllAsync<T>(T items) where T : IEnumerable<BaseModel>, new()
        {

            return await dbService.Db.UpdateAllAsync(items);
        }
        async public static Task<int> InsertOrUpdateAsync<T>(T item) where T : BaseModel, new()
        {
            bool isNewRecord = !dbTableService.Exists<T>(item.Gid);
            if (isNewRecord)
            {
                return await dbService.Db.InsertAsync(item);
            }
            else
                return await dbService.Db.UpdateAsync(item);
        }

        //Delete
        async public static Task<int> DeleteAsync<T>(T item) where T : BaseModel, new()
        {
            return await dbService.Db.DeleteAsync(item);
        }

        async public static Task<int> ExecuteQuery(string sql, params object[] args)
        {
            return await dbService.Db.ExecuteAsync(sql, args);
        }
    }


}
