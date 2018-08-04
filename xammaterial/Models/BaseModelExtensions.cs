
using Calibre.Helpers;
using Calibre.REST;
using Calibre.Models;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Calibre.DataModels;

namespace Calibre.Models
{

    public static class BaseModelExtensions
    {
        
        /*
        public static IdName Get(this IEnumerable<IdName> records, int id) 
        {
            return records.Where(x => x.Id == id).FirstOrDefault();
        }
        public static T Get<T>(this IEnumerable<T> records, int id) where T : BaseModel
        {
            return records.Where(x => x.Id == id).FirstOrDefault();
        }
        public static BaseModel Get(this IEnumerable<BaseModel> records, string gid)
        {
            return records.Where(x => x.Gid == gid).FirstOrDefault();
        }
        
            public async static Task<bool> PushToServerAsync(this BaseModel item)
        {
            bool isNewRecord = false;
            if (string.IsNullOrEmpty(item.Gid))
            {
                isNewRecord = true;
                item.Gid = Guid.NewGuid().ToString();
            }
            if (isNewRecord)
            {
                item.CreatedAt = DateTimeOffset.Now;
                item.UpdatedAt = DateTimeOffset.Now;
            }
            else
            {
                item.UpdatedAt = DateTimeOffset.Now;
            }

            RestService rest = new RestService();
            bool isSuccess = await rest.SaveAsync(item);
            if (isSuccess)
            {
                item.IsSyncPending = false;
                item.SyncedAt = DateTimeOffset.Now;
            }
            return isSuccess;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="parms"></param>
        /// <param name="format">data/json depending on the result of server</param>
        /// <returns></returns>
        public async static Task<bool> PullLatestAsync<T>(this ObservableRangeCollection<T> list, Dictionary<string, object> parms = null,string format = RestService.FORMAT_JSON)
        {
            list.Clear();
            RestService rest = new RestService();
            parms = parms ?? new Dictionary<string, object>();
            if (!parms.ContainsKey("MaxAvailableVersion"))
            {
                parms["MaxAvailableVersion"] = 0;
                parms["EmpId"] = Calibre.Settings.LoggedInUserId;
                parms["DayOfVisit"] = DateTime.Now.Day;
            }
            
            //var data = new { MaxAvailableVersion = 0 };
            var result = await rest.GetDataAsync<T>(parms,format);
            if (result.code == RestService.OK)
            {
                list.ReplaceRange(result.data);
            }
            return (result.code == RestService.OK);
        }
       
        public static void Save<T>(this ObservableRangeCollection<T> records, T item)
        {
            if (!records.Contains(item))
                records.Add(item);
            
        }
        public static void SaveRange<T>(this ObservableRangeCollection<T> records, IEnumerable<T> items)
        {
            foreach(var item in items)
            {
                records.Save(item);
            }
        }*/

    }

}