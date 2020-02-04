namespace DevOpsSync.WebApp.Services.Models.GitHub.Events
{
    public class PullRequestEvent
    {
        public string action { get; set; }
        public Pull_Request pull_request { get; set; }
    }

    public class Pull_Request
    {
        public string body { get; set; }
    }
}
