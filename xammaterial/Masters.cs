using Calibre.DataModels;
using Calibre.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Calibre.Models
{

    public class JobInfo : BaseModel
    {
        public int JobId { get; set; }
        public string JobName { get; set; }
        public string JobCode { get; set; }
    }

    /*
        public class State : ItemRecord
        {

            public static IEnumerable<State> All()
            {
                return App.MastersSyncDb.Table<State>().ToList();
            }

        }
        public class Bank : ItemRecord
        {

            public static IEnumerable<Bank> All()
            {
                return App.MastersSyncDb.Table<Bank>().ToList();
            }

        }
        public class District : ItemRecord
        {
            public int StateId { get; set; }
            public static IEnumerable<District> ForState(int stateId)
            {
                return App.MastersSyncDb.Table<District>().Where(x => x.StateId == stateId).ToList();

            }

        }
        public class Town : District
        {
            public int DistrictId { get; set; }
            new public static IEnumerable<Town> ForState(int stateId)
            {
                return App.MastersSyncDb.Table<Town>().Where(x => x.StateId == stateId).ToList();

            }
            public static IEnumerable<Town> ForDistrict(int districtId)
            {
                return App.MastersSyncDb.Table<Town>().Where(x => x.DistrictId == districtId).ToList();

            }
        }
        */
    public class Masters
    {
        public static ItemRecord[] ItemTypes = new ItemRecord[]
                   {
                        new ItemRecord {Id = 1,Name = "News"}
                        ,new ItemRecord {Id = 2,Name = "Event"}
                   };

        public static ItemRecord[] ItemCategories = new ItemRecord[]
                   {
                        new ItemRecord {Id = 1,Name = "Distributor"}
                        ,new ItemRecord {Id = 2,Name = "Government"}
                   };

        public static ItemRecord[] AssociationMemberTypes = new ItemRecord[]
                   {
                        new ItemRecord {Id = 1,Name = "President"}
                        ,new ItemRecord {Id = 2,Name = "Secretary"}
                        ,new ItemRecord {Id = 3,Name = "Board Member"}
                        ,new ItemRecord {Id = 4,Name = "General Member"}
                   };

        public static ItemRecord[] SegmentTypes = new ItemRecord[]
                    {
                        new ItemRecord {Id = 5,Name = "Consumer durables"}
                        ,new ItemRecord {Id = 3,Name = "FMCG - Beverages"}
                        ,new ItemRecord {Id = 1,Name = "FMCG - Foods"}
                        ,new ItemRecord {Id = 2,Name = "FMCG - Personal Care"}
                        ,new ItemRecord {Id = 6,Name = "Healthcare"}
                        ,new ItemRecord {Id = 8,Name = "Others"}
                        ,new ItemRecord {Id = 7,Name = "Stationary"}
                        ,new ItemRecord {Id = 4,Name = "Telecom"}
                    }

                 ;
       // public static List<State> States { get; set; } = new List<State>();

        public static ItemRecord[] TurnoverClasses = new ItemRecord[]

        {new ItemRecord {Id = 1,Name = "Class A : >50 Lacs"},new ItemRecord {Id = 2,Name = "Class B : 20-50 Lacs"},new ItemRecord {Id = 3,Name = "Class C : 10 -20 Lacs"},new ItemRecord {Id = 4,Name = "Class D : <10 Lacs"}};


        public static ItemRecord[] BusinessProfiles = new ItemRecord[]
        {
            new ItemRecord {Id = 2, Name = "C&F"},new ItemRecord {Id = 1,Name = "Direct Distributor"},new ItemRecord {Id = 3,Name = "Stockist"},new ItemRecord {Id = 4,Name = "Sub Dealer"}
        };
        public static ItemRecord[] FirmTypes = new ItemRecord[]
        {
            new ItemRecord {Id = 2, Name = "Partnership"},new ItemRecord {Id = 1,Name = "Sole Proprietor"}

        };
        public static ItemRecord[] ChannelTypes = new ItemRecord[]
       {
            new ItemRecord {Id = 1, Name = "General Trade"},new ItemRecord {Id = 2,Name = "Modern Trade"},new ItemRecord {Id = 3,Name = "Institutional"},new ItemRecord {Id = 4,Name = "CSD"}

       };



     /*

        public static IEnumerable<T> Get<T>(int id) where T : ItemRecord, new()
        {
            return App.MastersSyncDb.Table<T>().Where(x => x.Id == id).ToList();
        }
        */
    }



}
