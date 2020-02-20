using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    public class SpreadsheetState
    {
        public string Username
        { get; set; }
        public string Password
        { get; set; }
        public IEnumerable<string> AvailableSpreadsheets
        { get; set; }
        public string SelectedSpreadsheet
        { get; set; }
        public Dictionary<string, string> RecentChanges
        { get; set; }
        public bool Connected
        { get; set; }
    }
}
