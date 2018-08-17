namespace SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport
{
    public class TestConfiguration
    {
        public string BrowserStackUser { get; set; }

        public string BrowserStackKey { get; set; }

        public TestUser ViewAccessUser { get; set; }
        public TestUser EditAccessUser { get; set; }
        public TestUser SubmitAccessUser { get; set; }

        public string EmployerId { get; set; }
    }
}