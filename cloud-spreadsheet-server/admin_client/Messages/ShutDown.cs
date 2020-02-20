using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdministrativeClient
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ShutDown
    {
        [JsonProperty]
        string type = "shutdown";
    }
}
