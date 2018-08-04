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

        //Local created id, unique across whole server database
        [PrimaryKey]
        [Collation("NOCASE")]
        public string Gid { get; set; }

        //Server created id on table, retrieved upon sync from server
        //Do not rely on this on client side for lookups
        [Indexed]
        public int Id { get; set; }

        //server created version/timestamp 
        [Indexed]
        public Int64 Version { get; set; } //for storing timestamp/byte[] for comparable format

        public DateTimeOffset? CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }

        [JsonIgnore]
        public DateTimeOffset? SyncedAt { get; set; }

        [JsonIgnore]
        [Indexed]
        public bool IsSyncPending { get; set; } = true;

        public bool Deleted { get; set; } = false;



    }


}