using System;
using System.Collections.Generic;

namespace DevOpsSync.WebApp.API.Models.GitHub.Events
{
    public class PushEvent
    {
        public IEnumerable<Commit> commits { get; set; }
    }

    public class Commit
    {
        public string id { get; set; }
        public string tree_id { get; set; }
        public bool distinct { get; set; }
        public string message { get; set; }
        public DateTime timestamp { get; set; }
        public string url { get; set; }
        public Author author { get; set; }
        public Committer committer { get; set; }
        public object[] added { get; set; }
        public object[] removed { get; set; }
        public string[] modified { get; set; }
    }

    public class Author
    {
        public string name { get; set; }
        public string email { get; set; }
        public string username { get; set; }
    }

    public class Committer
    {
        public string name { get; set; }
        public string email { get; set; }
        public string username { get; set; }
    }
}
