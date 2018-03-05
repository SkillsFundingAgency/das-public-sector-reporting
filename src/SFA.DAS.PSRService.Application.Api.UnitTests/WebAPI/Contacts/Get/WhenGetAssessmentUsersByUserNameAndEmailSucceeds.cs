//namespace SFA.DAS.PSRService.Application.Api.UnitTests.WebAPI._reportContoller
//{
//    using Machine.Specifications;
//    using SFA.DAS.PSRService.Application.Api.Controllers;
//    using FizzWare.NBuilder;
//    using SFA.DAS.PSRService.ViewModel.Models;
//    using System.Threading.Tasks;
//    using FluentAssertions;

//    [Subject("AssessorService")]
//    public class WhenGetAssessmentUsersByUserNameAndEmailSucceeds : WhenGetAssessmentUsersTestBase
//    {
//        private static Report _organisationQueryViewModels;
      
//        Establish context = () =>
//        {
//            Setup();

//            _organisationQueryViewModels = Builder<Report>.CreateNew().Build();

//            ContactQueryRepository.Setup(q => q.GetContact(Moq.It.IsAny<string>()))
//                .Returns(Task.FromResult((_organisationQueryViewModels)));

//            ContactQueryController = new ContactQueryController(                
//                ContactQueryRepository.Object, 
//                Logger.Object);
//        };

//        Because of = () =>
//        {
//            Result = ContactQueryController.GetContactsByUserName("TestUser").Result;
//        };

//        Machine.Specifications.It verify_succesfully = () =>
//        {
//            var result = Result as Microsoft.AspNetCore.Mvc.OkObjectResult;
//            result.Should().NotBeNull();
//        };

//         Machine.Specifications.It should_be_correct_value  = () =>
//         {
//             var result = Result as Microsoft.AspNetCore.Mvc.OkObjectResult;
//             (result.Value is Report).Should().BeTrue();             
//         };
//    }
//}
