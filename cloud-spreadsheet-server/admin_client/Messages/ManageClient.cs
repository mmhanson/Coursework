using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdministrativeClient
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ManageClient
    {
        [JsonProperty]
        string type;

        [JsonProperty]
        string action;

        [JsonProperty]
        string username;

        public ManageClient(string action, string username)
        {
            this.type = "manageclient";
            this.action = action;
            this.username = username;
        }
    }
}
