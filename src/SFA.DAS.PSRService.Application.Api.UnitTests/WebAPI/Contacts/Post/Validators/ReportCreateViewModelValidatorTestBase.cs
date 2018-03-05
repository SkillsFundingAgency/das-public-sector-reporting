//namespace SFA.DAS.PSRService.Application.Api.UnitTests.Validators.UkPrnValidator
//{
//    using AssessorService.Api.Types.Models;
//    using Microsoft.Extensions.Localization;
//    using Moq;
//    using SFA.DAS.PSRService.Application.Api.Consts;
//    using SFA.DAS.PSRService.Application.Api.Validators;
//    using SFA.DAS.PSRService.Application.Interfaces;

//    public class ReportCreateViewModelValidatorTestBase
//    {
//        protected static ReportCreateViewModelValidator ReportCreateViewModelValidator;
//        protected static Mock<IReportRepository> ReportRepositoryMock;      
//        protected static CreateReportRequest ReportCreateViewModel;
//        protected static Mock<IReportQueryRepository> ReportQueryRepository;

//        public static void Setup()
//        {
//            var stringLocalizerMock = new Mock<IStringLocalizer<ReportCreateViewModelValidator>>();
//            string key = ResourceMessageName.NoAssesmentProviderFound;
//            var localizedString = new LocalizedString(key, "10000000");
//            stringLocalizerMock.Setup(q => q[Moq.It.IsAny<string>(), Moq.It.IsAny<string>()]).Returns(localizedString);

//            ReportRepositoryMock = new Mock<IReportRepository>();

//            ReportQueryRepository = new Mock<IReportQueryRepository>();

//            ReportCreateViewModelValidator = new ReportCreateViewModelValidator(stringLocalizerMock.Object, ReportQueryRepository.Object);               
//        }
//    }
//}
