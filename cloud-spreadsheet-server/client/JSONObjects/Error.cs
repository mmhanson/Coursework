using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    public class Error : MessageParent
    {
        [JsonProperty]
        private string type = "error";

        [JsonProperty]
        private int code;

        [JsonProperty]
        private string source;

        public int GetCode()
        {
            return code;
        }

        public string GetSource()
        {
            return source;
        }
    }
}
