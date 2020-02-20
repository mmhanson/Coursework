using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdministrativeClient
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Update
    {
        [JsonProperty]
        public string type;

        [JsonProperty]
        public string username
        { get; private set; }

        [JsonProperty]
        public string status
        { get; private set; }

        [JsonProperty]
        public string cell
        { get; private set; }
    }
}
