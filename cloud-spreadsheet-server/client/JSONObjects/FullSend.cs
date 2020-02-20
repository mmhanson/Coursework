using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    public class FullSend : MessageParent
    {
        [JsonProperty]
        private string type = "full send";

        [JsonProperty]
        Dictionary<string, string> spreadsheet;

        /// <summary>
        /// Returns a dictionary of cellNames mapped to their values.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetSpreadsheet()
        {
            return spreadsheet;
        }
    }
}
