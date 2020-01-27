namespace DevOpsSync.WebApp.API
{
    public class Settings
    {
        public ClientSettings GitHub { get; set; }
        public ClientSettings Slack { get; set; }
    }

    public class ClientSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUrl { get; set; }
    }
}
