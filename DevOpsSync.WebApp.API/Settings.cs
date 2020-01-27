namespace DevOpsSync.WebApp.API
{
    public class Settings
    {
        public GitHubSettings GitHub { get; set; }
    }

    public class GitHubSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUrl { get; set; }
    }
}
