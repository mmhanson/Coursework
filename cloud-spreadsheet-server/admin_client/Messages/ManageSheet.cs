using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdministrativeClient
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ManageSheet
    {
        [JsonProperty]
        string type;

        [JsonProperty]
        string action;

        [JsonProperty]
        string spreadsheet;

        public ManageSheet(string action, string spreadsheet)
        {
            this.type = "managesheet";
            this.action = action;
            this.spreadsheet = spreadsheet;
        }
    }
}
