namespace SFA.DAS.PSRService.Web.Specflow.Tests.consts
{
    public static class PageUrls
    {
        public const string Home = "/Home";
        public const string ReportCreate = "/Report/Create";
        public const string ReportEdit = "/Report";
        public const string ReportSummary = "/Report/Summary";
        public const string ReportConfirmSubmision = "/Report/Confirm";
        public const string ReportEditComplete = "/Report/EditComplete";
        public const string ReportAlreadySubmitted = "/Report/AlreadySubmitted";
        public const string ReportPreviouslySubmittedList = "/Report/List";
        public const string ReportSubmitConfirmation = "/Report/SubmitConfirmation";
        public const string ReportOrganisationName = "/Report/OrganisationName";
        public const string ReportHistory = "/Report/History";
        public const string QuestionEdit = "/Question/{0}";
    }

    public static class QuestionPageUrls
    {
        private const string QuestionBaseUrl = "/Question";
        public const string YourEmployees = QuestionBaseUrl + "/YourEmployees";
        public const string YourApprentices = QuestionBaseUrl + "/YourApprentices";
        public const string FullTimeEquivalent = QuestionBaseUrl + "/FullTimeEquivalent";
        public const string OutlineActions = QuestionBaseUrl + "/OutlineActions";
        public const string Challenges = QuestionBaseUrl + "/Challenges";
        public const string TargetPlans = QuestionBaseUrl + "/TargetPlans";
        public const string AnythingElse = QuestionBaseUrl + "/AnythingElse";
    }
}
