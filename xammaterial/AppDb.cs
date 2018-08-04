using Calibre;
using Calibre.Db;
using Calibre.Models;
using Calibre.REST;
using Calibre.Xam.Log;
using Calibre.Xam.SyncServices;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace xammaterial.Db
{
    /// <summary>
    /// Place for application specific database related tasks
    /// </summary>
    class AppDb
    {

        public void dbLog(object db, LogType type, string message)
        {
            Debug.WriteLine(message);
        }
        public void ClearTables()
        {

           // dbService.SyncDb.DropTable<ImageCategory>();
           
        }
        public void InitTables(dbService db)
        {

            dbService.Db.CreateTablesAsync(types: new Type[] {
                typeof(JobInfo)

            }).ConfigureAwait(false);



        }

    }
}
