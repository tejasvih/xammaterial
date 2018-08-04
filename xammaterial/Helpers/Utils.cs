using System;
using System.Collections.Generic;
using System.Text;

namespace Calibre.Xam.Helpers
{
    public class Utils
    {
        public static D ConvertViaJson<S,D>(S src)
        {
            var o = Newtonsoft.Json.JsonConvert.SerializeObject(src);
            D dest = Newtonsoft.Json.JsonConvert.DeserializeObject<D>(o);
            return dest;
        }
    }
}
