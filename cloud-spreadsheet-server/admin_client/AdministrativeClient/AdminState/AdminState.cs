using System;
using System.Collections.Generic;
using System.Text;

namespace AdministrativeClient
{
    public class AdminState
    {
        private Stack<string> changesHistory;
        private Queue<string> changesSequential;

        private Dictionary<string, string> clientList;
        public bool Connected
        { get; set; }

        public AdminState()
        {
            changesHistory = new Stack<string>();
            changesSequential = new Queue<string>();
            clientList = new Dictionary<string, string>();
        }

        public void AddChange(string change)
        {
            changesHistory.Push(change);
            changesSequential.Enqueue(change);
        }

        public string GetLastChange()
        {
            return (changesHistory.Count > 0)?changesHistory.Peek():"";
        }

        public IEnumerable<string> GetChangesInOrder()
        {
            return changesSequential.ToArray();
        }

        public void SetClientList(Dictionary<string, string> list)
        {
            this.clientList = list;
        }

        public Dictionary<string, string> GetClientList()
        {
            return this.clientList;
        }
    }
}
