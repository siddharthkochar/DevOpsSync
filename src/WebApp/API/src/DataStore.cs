using System.Collections.Generic;

namespace DevOpsSync.WebApp.API
{
    public interface IDataStore
    {
        Dictionary<string, string> Storage { get; set; }
        Dictionary<string, object> ObjectStorage { get; set; }
    }

    public class DataStore : IDataStore
    {
        public DataStore()
        {
            Storage = new Dictionary<string, string>();
            ObjectStorage = new Dictionary<string, object>();
        }

        public Dictionary<string, string> Storage { get; set; }
        public Dictionary<string, object> ObjectStorage { get; set; }
    }
}
