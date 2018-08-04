using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Calibre.DataModels
{
  
    public class User : BaseModel
    {
        
        public string UserName { get; set; }
        public string MobileNumber { get; set; }
        
        public bool IsActive { get; set; }
        public string FullName { get; set; }
        public string FullNameInEnglish { get; set; }

        public bool HasEditingRights { get; set; }
        

        [JsonIgnore]
        public DateTimeOffset LoggedInAt { get; set; }
        //GetUserByMobileNumber
    }


}