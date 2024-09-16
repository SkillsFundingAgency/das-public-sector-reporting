using SFA.DAS.PSRService.Web.Controllers;

namespace SFA.DAS.PSRService.IntegrationTests.ReportSubmission.Given_I_Have_Created_A_Report.
    And_I_Have_Reported_All_Mandatory_Numbers;

public abstract class And_I_Have_Reported_All_Mandatory_Numbers
    : Given_I_Have_Created_A_Report
{
    public And_I_Have_Reported_All_Mandatory_Numbers(bool isLocalAuthority) : base(isLocalAuthority){}

    protected override void Given()
    {
        base.Given();

        BuildAndSubmitAllMandatoryNumbers();
    }

    private void BuildAndSubmitAllMandatoryNumbers()
    {
        QuestionController
            .Submit(
                new ReportNumbersAnswersBuilder()
                    .BuildValidYourEmployeesAnswers()
                    .ForReportingPeriod(TestHelper.CurrentPeriod));

        QuestionController
            .Submit(
                new ReportNumbersAnswersBuilder()
                    .BuildValidYourApprenticesAnswers()
                    .ForReportingPeriod(TestHelper.CurrentPeriod));
    }
}