using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SpreadsheetList : MessageParent
    {
        [JsonProperty]
        public string type = "list";

        [JsonProperty]
        public string[] spreadsheets;
    }
}
