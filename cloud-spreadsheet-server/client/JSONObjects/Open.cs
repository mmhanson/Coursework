using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Open : MessageParent
    {
        [JsonProperty]
        private string type;
        [JsonProperty]
        private string name;
        [JsonProperty]
        private string username;
        [JsonProperty]
        private string password;

        public Open(string name, string username, string password)
        {
            this.type = "open";
            this.name = name;
            this.username = username;
            this.password = password;
        }

        public Open()
        {
        }

        public void setName(string name)
        {
            this.name = name;
        }
        public string getName()
        {
            return this.name;
        }

        public void setUsername(string username)
        {
            this.username = username;
        }
        public string getUsername()
        {
            return this.username;
        }

        public void setPassword(string password)
        {
            this.password = password;
        }
        public string getPassword()
        {
            return this.password;
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}

