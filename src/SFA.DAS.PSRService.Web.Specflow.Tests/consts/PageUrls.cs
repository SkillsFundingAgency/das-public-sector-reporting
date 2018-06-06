namespace SFA.DAS.PSRService.Web.Specflow.Tests.consts
{
    public static class PageUrls
    {
        public const string ReportCreate = "/Report/Create";
        public const string ReportEdit = "/Report";
        public const string ReportSummary = "/Report/Summary";
        public const string ReportConfirmSubmision = "/Report/Confirm";
        public const string ReportPreviouslySubmittedList = "/Report/List";
        public const string QuestionEdit = "/Question/{0}";
    }

    public static class QuestionPAgeUrls
    {
        private const string QuesionBaseUrl = "/Question";
        public const string YourEmployees = QuesionBaseUrl + "/";
    }
}
