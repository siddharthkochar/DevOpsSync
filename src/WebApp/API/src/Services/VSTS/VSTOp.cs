namespace DevOpsSync.WebApp.API.Services.VSTS
{
    public class VSTSOp
    {
        private VSTSOp()
        {
            New = nameof(New);
            Done = nameof(Done);
            InProgress = nameof(InProgress);
            InVerify = nameof(InVerify);
            Approved = nameof(Approved);
            Committed = nameof(Committed);
        }

        public string New { get; }
        public string Done { get; }
        public string InProgress { get; }
        public string InVerify { get; }
        public string Committed { get; }
        public string Approved { get; }

        public static VSTSOp GetInstance() => new VSTSOp();
    }
}