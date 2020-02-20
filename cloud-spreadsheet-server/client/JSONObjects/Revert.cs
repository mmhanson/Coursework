using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    [JsonObject(MemberSerialization.OptIn)]
  public class Revert : MessageParent
    {
        [JsonProperty]
        public string type = "revert";

        [JsonProperty]
        private string cell;

        public Revert(string cell)
        {
            this.cell = cell;
        }

        public void setCell(string cell)
        {
            this.cell = cell;
        }
        public string getCell()
        {
            return this.cell;
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

