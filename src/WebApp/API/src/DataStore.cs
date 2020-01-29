using System.Collections.Generic;

namespace DevOpsSync.WebApp.API
{
    public interface IDataStore
    {
        Dictionary<string, string> Storage { get; set; }
    }

    public class DataStore : IDataStore
    {
        public DataStore()
        {
            Storage = new Dictionary<string, string>();
        }

        public Dictionary<string, string> Storage { get; set; }
    }
}
