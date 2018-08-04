using Calibre.Models;
using Calibre.Db;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Calibre.DataModels
{

    public partial class BaseModel : ObservableObject
    {

        //Methods
        public BaseModel() { }
        public virtual bool Init() { return true; }

        public virtual void AfterLoad() { }
        public virtual void BeforeSave() { }
        public virtual void BeforeInsert() { }
        public virtual void BeforeUpdate() { }
        public virtual void AfterInsert() { }
        public virtual void AfterUpdate() { }
        public virtual void AfterSave() { }

        public virtual void BeforeSync() { }
        public virtual void AfterSync() { }



        public virtual void DirectUpdate()
        {
            dbTableService.Update(this);
        }
        async public virtual Task DirectUpdateAsync()
        {
            await dbTableService.UpdateAsync(this);
        }

        
        public virtual bool PrepareToSave()
        {
            SyncedAt = null;
            IsSyncPending = true;
            bool isNewRecord = false;
            if (string.IsNullOrEmpty(Gid))
            {
                isNewRecord = true;
                Gid = Guid.NewGuid().ToString();
            }
            if (isNewRecord)
            {
                CreatedAt = DateTimeOffset.Now;
                UpdatedAt = DateTimeOffset.Now;
                CreatedBy = Settings.LoggedInUserId;
                UpdatedBy = Settings.LoggedInUserId;
            }
            else
            {
                UpdatedAt = DateTimeOffset.Now;
                UpdatedBy = Settings.LoggedInUserId;
            }
            return isNewRecord;
        }
        public virtual string Save()
        {
            SaveRecord(PrepareToSave());
            return Gid;
        }
        protected virtual string SaveRecord(bool isNewRecord)
        {

            BeforeSave();
            if (isNewRecord)
            {
                BeforeInsert();
                dbTableService.Insert(this);
                AfterInsert();
            }
            else
            {
                BeforeUpdate();
                dbTableService.Update(this);
                AfterUpdate();
            }
            AfterSave();
            return Gid;
        }
        //Async Tasks
        public async virtual Task<string> SaveAsync()
        {
            await SaveRecordAsync(PrepareToSave());
            return Gid;
        }
        public async virtual Task<string> SaveRecordAsync(bool isNewRecord)
        {

            BeforeSave();
            if (isNewRecord)
            {
                BeforeInsert();
                await dbTableService.InsertAsync(this);
                AfterInsert();
            }
            else
            {
                BeforeUpdate();
                await dbTableService.UpdateAsync(this);
                AfterUpdate();
            }
            AfterSave();
            return Gid;
        }

       

    }


}