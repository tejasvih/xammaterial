using Calibre.DataModels;
using Calibre.Db;
using Calibre.REST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calibre.Xam.SyncServices
{
    public class SyncService
    {
        public async static Task<bool> AuthenticateUser(string userid, string password)
        {

            Calibre.Settings.LoggedInUserId = 0;
            Calibre.Settings.LoggedInUserName = null;
            Calibre.Settings.LoggedInUserMobileNumber = null;
            Calibre.Settings.LoggedInUserFullName = null;
            Calibre.Settings.LoggedInUserFullNameInEnglish = null;
            Settings.HasEditingRights = false;

            RestService rest = RestService.GetInstance();
            if (!(await RestService.IsServerReachableAndRunning()))
                return false;

            var data = new { UserName = userid, Password = password };
            var result = await rest.ExecuteApiAsync<User>("AuthenticateUser", data);
            if (result.code == RestService.OK)
            {
                if (result.data?.Count > 0)
                {
                    var item = result.data.First();
                    if (item.Id > 0)
                    {
                        Calibre.Settings.LoggedInUserId = item.Id;
                        Calibre.Settings.LoggedInUserName = item.UserName;
                        Settings.HasEditingRights = item.HasEditingRights;
                        Calibre.Settings.LoggedInUserMobileNumber = item.MobileNumber;
                        Calibre.Settings.LoggedInUserFullName = item.FullName;
                        Calibre.Settings.LoggedInUserFullNameInEnglish = item.FullName;
                        return true;
                    }
                }
            }
            return false;
        }
        public static Int64 GetMaxVersion<T>() where T : BaseModel, new()
        {
            Int64 retVal = 0;
            if (dbService.SyncDb.Table<T>().Count() > 0)
                retVal = dbService.SyncDb.Table<T>().Max(x => x.Version);
            return retVal;
        }
        /* public static int PullLatest<T>() where T : BaseModel, new()
         {
             RestService rest = RestService.GetInstance();
             var data = new { MaxAvailableVersion = GetMaxVersion<T>() };
             var result = rest.GetDataAsync<T>(data).Result;
             if (result.code == RestService.OK)
             {
                 foreach (var item in result.data)
                 {
                     item.SyncedAt = DateTimeOffset.Now;
                     item.IsSyncPending = false;
                     dbTableService.InsertOrUpdate<T>(item);
                 }
                 return result.data.Count;
             }
             return -1;
         }
         public static bool PushToServer(BaseModel item)
         {
             RestService rest = RestService.GetInstance();
             bool isSuccess = rest.SaveAsync(item).Result;
             if (isSuccess)
             {
                 item.IsSyncPending = false;
                 item.SyncedAt = DateTimeOffset.Now;
                 item.DirectUpdate();
             }
             return isSuccess;
         }
         public static bool PushAllPendingToServer<T>() where T : BaseModel, new()
         {
             bool isSuccess = false;
             RestService rest = RestService.GetInstance();
             var items = dbService.SyncDb.Table<T>().Where(x => x.IsSyncPending).ToList();
             if (items.Count > 0)
             {
                 isSuccess = rest.SaveAsync(items, typeof(T)).Result;
             }

             if (isSuccess)
             {
                 foreach (var item in items)
                 {
                     item.IsSyncPending = false;
                     item.SyncedAt = DateTimeOffset.Now;

                 }
                 dbTableService.UpdateAll(items);
             }
             return isSuccess;
         }
         */

        //Async methods

        /// <summary>
        /// Pulls non base model type of data
        /// Used to get arbitrary class records
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async static Task<(int code, List<T> data)> GetRecordsAsync<T>(object data = null, string format = RestService.FORMAT_DATA,string apiName = null) where T : new()
        {
            RestService rest = RestService.GetInstance();
            var result = await rest.GetDataAsync<T>(data, format, apiName);
            return result;
        }

        /// <summary>
        /// Gets all new records from server
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async static Task<int> PullLatestAsync<T>() where T : BaseModel, new()
        {
            RestService rest = RestService.GetInstance();
            var data = new {
                MaxAvailableVersion = GetMaxVersion<T>(),
                EmpId = Settings.LoggedInUserId
            };
            var result = await rest.GetDataAsync<T>(data);
            if (result.code == RestService.OK)
            {
                if (result.data != null)
                {
                    foreach (var item in result.data)
                    {
                        item.SyncedAt = DateTimeOffset.Now;
                        item.IsSyncPending = false;
                        await dbTableService.InsertOrUpdateAsync<T>(item);
                    }
                }
                /*else
                    return RestService.NO_DATA;*/
                //return result.data.Count;
            }
            return result.code;
        }
        /// <summary>
        /// Fetches records from server by passing custom parameters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parms"></param>
        /// <returns></returns>
        public async static Task<int> PullLatestAsync<T>(Dictionary<string, object> parms = null) where T : BaseModel, new()
        {
            RestService rest = RestService.GetInstance();
            //var data = new { MaxAvailableVersion = GetMaxVersion<T>() };
            parms = parms ?? new Dictionary<string, object>();
            if (!parms.ContainsKey("MaxAvailableVersion"))
            {
                parms["MaxAvailableVersion"] = GetMaxVersion<T>();
                parms["EmpId"] = Settings.LoggedInUserId;
            }

            //var data = new { MaxAvailableVersion = 0 };
            
            var result = await rest.GetDataAsync<T>(parms);
            if (result.code == RestService.OK)
            {
                if (result.data != null)
                {
                    foreach (var item in result.data)
                    {
                        item.SyncedAt = DateTimeOffset.Now;
                        item.IsSyncPending = false;
                        await dbTableService.InsertOrUpdateAsync<T>(item);
                    }
                }
                /*else
                    return RestService.NO_DATA;*/
                //return result.data.Count;
            }
            return result.code;
        }
        /// <summary>
        /// Saves individual item on server
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async static Task<int> PushToServerAsync(BaseModel item)
        {
            RestService rest = RestService.GetInstance();
            item.BeforeSync();
            bool isSuccess = await rest.SaveAsync(item);
            if (isSuccess)
            {
                item.IsSyncPending = false;
                item.SyncedAt = DateTimeOffset.Now;
                item.AfterSync();
                await item.DirectUpdateAsync();
            }
            return isSuccess ? RestService.OK : RestService.ERROR;
        }
        /// <summary>
        /// Saves all unsynced items to server
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async static Task<int> PushAllPendingToServerAsync<T>() where T : BaseModel, new()
        {
            bool isSuccess = false;
            RestService rest = RestService.GetInstance();
            var items = await dbService.Db.Table<T>().Where(x => x.IsSyncPending).ToListAsync();
            if (items.Count > 0)
            {
                isSuccess = await rest.SaveAsync(items, typeof(T));
            }
            else
                return RestService.OK;

            if (isSuccess)
            {
                foreach (var item in items)
                {
                    item.IsSyncPending = false;
                    item.SyncedAt = DateTimeOffset.Now;
                }
                await dbTableService.UpdateAllAsync(items);
            }
            return isSuccess ? RestService.OK : RestService.ERROR;
        }
        
    }
}
