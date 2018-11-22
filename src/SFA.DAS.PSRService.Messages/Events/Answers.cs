namespace SFA.DAS.PSRService.Messages.Events
{
    public class Answers
    {
        public string  OrganisationName { get; set; }
        public YourEmployees YourEmployees { get; set; }
        public YourApprentices YourApprentices { get; set; }
        public string FullTimeEquivalents { get; set; }
        public string OutlineActions { get; set; }
        public string Challenges { get; set; }
        public string TargetPlans { get; set; }
        public string AnythingElse { get; set; }
    }
}