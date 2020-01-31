namespace DevOpsSync.WebApp.API.Services.VSTS
{
    public static class Constants
    {
        public class VSTSOp
        {
            private VSTSOp()
            {
                New = nameof(New);
                Done = nameof(Done);
                InProgress = nameof(InProgress);
                InVerify = nameof(InVerify);
            }

            public string New { get; }
            public string Done { get; }
            public string InProgress { get; }
            public string InVerify { get; }

            public static VSTSOp GetInstance() => new VSTSOp();
        }

        public static VSTSOp VSTSOpEnum = VSTSOp.GetInstance();
    }
}