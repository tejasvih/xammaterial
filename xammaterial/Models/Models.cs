using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Calibre.Models
{
    public class IdName
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class ResultRecord
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Gid { get; set; }
    }
    public class ItemRecord
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameInEnglish { get; set; }



    }
    public static class RecordExtensions
    {
        public static ItemRecord Get(this ItemRecord[] records, int id)
        {
            return records.Where(x => x.Id == id).FirstOrDefault();
        }
    }
    public class ItemSubRecord : ItemRecord
    {
        public int MasterId { get; set; }

    }
    

}
