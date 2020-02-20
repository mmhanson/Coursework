using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Edit : MessageParent
    {
        [JsonProperty]
        private string type;
        [JsonProperty]
        private string cell;
        [JsonProperty]
        private string value;
        [JsonProperty]
        private List<string> dependencies;

        public Edit(string cell, string value, List<string> dependencies)
        {
            this.type = "edit";
            this.cell = cell;
            this.value = value;
            this.dependencies = dependencies;
        }

        public void setCell(string cell)
        {
            this.cell = cell;
        }
        public string getCell()
        {
            return this.cell;
        }

        public void setValue(string value)
        {
            this.value = value;
        }
        public string getValue()
        {
            return this.value;
        }

        public void setDependencies(string value)
        {
            double num;
            if (value.StartsWith("="))
            {
                string[] list = value.Split();
                foreach (string each in list)
                {

                    if (each == "+" || each == "-" || each == "/" || each == "*") continue;

                    else if (Double.TryParse(each, out num)) continue;
                    // add the dependecies variables into the dependencies list
                    else
                    {
                        dependencies.Add(each);
                    }
                }

            }


        }
        public List<string> getDependencies()
        {
            return this.dependencies;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}

