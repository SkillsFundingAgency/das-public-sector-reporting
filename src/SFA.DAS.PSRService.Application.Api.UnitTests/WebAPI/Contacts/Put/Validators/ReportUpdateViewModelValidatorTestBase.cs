//namespace SFA.DAS.PSRService.Application.Api.UnitTests.WebAPI.ReportContoller.Put.Validators
//{
//    using AssessorService.Api.Types.Models;
//    using Microsoft.Extensions.Localization;
//    using Moq;
//    using SFA.DAS.PSRService.Application.Api.Consts;
//    using SFA.DAS.PSRService.Application.Api.Validators;
//    using SFA.DAS.PSRService.Application.Interfaces;

//    public class ReportUpdateViewModelValidatorTestBase
//    {
//        protected static ReportUpdateViewModelValidator ReportUpdateViewModelValidator;
//        protected static Mock<IReportRepository> ReportRepositoryMock;      
//        protected static CreateReportRequest ReportCreateViewModel;

//        public static void Setup()
//        {
//            var stringLocalizerMock = new Mock<IStringLocalizer<ReportUpdateViewModelValidator>>();
//            string key = ResourceMessageName.NoAssesmentProviderFound;
//            var localizedString = new LocalizedString(key, "10000000");
//            stringLocalizerMock.Setup(q => q[Moq.It.IsAny<string>(), Moq.It.IsAny<string>()]).Returns(localizedString);

//            ReportRepositoryMock = new Mock<IReportRepository>();

//            ReportUpdateViewModelValidator = new ReportUpdateViewModelValidator(stringLocalizerMock.Object);               
//        }
//    }
//}
