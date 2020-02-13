namespace DevOpsSync.WebApp.Utility
{
    public class Settings
    {
        public string AppRootUrl { get; set; }
        public ClientSettings GitHub { get; set; }
        public ClientSettings Slack { get; set; }
        public ClientSettings VSTS { get; set; }

    }

    public class ClientSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUrl { get; set; }
    }
}
