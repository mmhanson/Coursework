using Newtonsoft.Json;
using System;

namespace AdministrativeClient
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ChangePassword
    {
        [JsonProperty]
        string type;

        [JsonProperty]
        string username;

        [JsonProperty]
        string newpw;

        public ChangePassword(string username, string password)
        {
            type = "change";
            this.username = username;
            this.newpw = password;
        }
    }
}
