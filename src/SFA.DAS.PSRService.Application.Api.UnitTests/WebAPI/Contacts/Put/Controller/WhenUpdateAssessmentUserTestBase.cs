namespace SFA.DAS.PSRService.Application.Api.UnitTests.WebAPI.Contacts.Contoller.Put
{
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;
    using Moq;
    using SFA.DAS.PSRService.Application.Api.Consts;
    using SFA.DAS.PSRService.Application.Api.Controllers;
    using SFA.DAS.PSRService.Application.Api.Validators;
    using SFA.DAS.PSRService.Application.Interfaces;

    public class WhenUpdateAssessmentUserTestBase
    {
        protected static Mock<IReportRepository> ContactRepository;
        protected static Mock<IStringLocalizer<ReportController>> StringLocalizer;
        protected static IActionResult Result;
        protected static UkPrnValidator UkPrnValidator;
        protected static Mock<ILogger<ReportController>> Logger;
        protected static Mock<IMediator> Mediator;

        protected static ReportController _reportContoller;

        protected static void Setup()
        {
            ContactRepository = new Mock<IReportRepository>();

            StringLocalizer = new Mock<IStringLocalizer<ReportController>>();
            string key = ResourceMessageName.NoAssesmentProviderFound;
            var localizedString = new LocalizedString(key, "10000000");
            StringLocalizer.Setup(q => q[Moq.It.IsAny<string>(), Moq.It.IsAny<int>()]).Returns(localizedString);
         

            Logger = new Mock<ILogger<ReportController>>();
            Mediator = new Mock<IMediator>();            
        }
    }
}
