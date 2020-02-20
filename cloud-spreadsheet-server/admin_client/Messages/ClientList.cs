using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdministrativeClient
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ClientList
    {
        [JsonProperty]
        private string type = "clientlist";

        [JsonProperty]
        private Dictionary<string, string> list;

        public Dictionary<string, string> GetList()
        {
            return list;
        }
    }
}
