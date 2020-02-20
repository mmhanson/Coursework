using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    [JsonObject(MemberSerialization.OptIn)]
   public class Undo : MessageParent
    {
        [JsonProperty]
        string type = "undo";
        
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

