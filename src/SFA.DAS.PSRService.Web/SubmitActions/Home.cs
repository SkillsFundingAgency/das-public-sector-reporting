namespace SFA.DAS.PSRService.Web.SubmitActions
{
    public static class Home
    {
        static Home()
        {
            Edit = new SubmitAction("edit", "report", "edit");
            List = new SubmitAction("list", "report", "list");
            Create = new SubmitAction("create", "report", "create");
            View = new SubmitAction("view", "report", "summary");
            AlreadySubmitted = new SubmitAction("alreadysubmitted", "report", "alreadysubmitted");
        }

        public static SubmitAction AlreadySubmitted { get; }
        public static SubmitAction Edit { get; }
        public static SubmitAction List { get; }
        public static SubmitAction Create { get; }
        public static SubmitAction View { get; }
    }
}