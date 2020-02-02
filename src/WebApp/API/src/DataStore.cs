using System.Collections.Generic;

namespace DevOpsSync.WebApp.API
{
    public interface IDataStore
    {
        Dictionary<string, object> Storage { get; set; }
    }

    public class DataStore : IDataStore
    {
        public DataStore()
        {
            Storage = new Dictionary<string, object>();
        }

        public Dictionary<string, object> Storage { get; set; }
    }
}
