//namespace SFA.DAS.PSRService.Application.Api.UnitTests.WebAPI.ContactContoller.Post.Handlers
//{
//    using FizzWare.NBuilder;
//    using FluentAssertions;
//    using Machine.Specifications;
//    using Moq;
//    using SFA.DAS.PSRService.Application.ContactHandlers;
//    using SFA.DAS.PSRService.Application.Interfaces;
//    using System;
//    using System.Threading;
//    using System.Threading.Tasks;
//    using AssessorService.Api.Types.Models;
//    using Domain;

//    [Subject("AssessorService")]
//    public class WhenOrganisationHasExistingContractsCreateContactHandlerSucceeds
//    {
//        private static CreateReportHandler CreateReportHandler;
//        protected static Mock<IReportRepository> ContactRepositoryMock;
//        protected static Mock<IOrganisationRepository> OrganisationRepositoryMock;
//        protected static Mock<IOrganisationQueryRepository> OrganisationQueryRepositoryMock;
//        protected static ContactCreateDomainModel ContactCreateDomainModel;
//        protected static Report ContactQueryViewModel;
//        protected static CreateReportRequest ContactCreateViewModel;
//        protected static Report _result;

//        Establish context = () =>
//        {
//            MappingBootstrapper.Initialize();

//            ContactRepositoryMock = new Mock<IReportRepository>();
//            OrganisationRepositoryMock = new Mock<IOrganisationRepository>();
//            OrganisationQueryRepositoryMock = new Mock<IOrganisationQueryRepository>();

//            OrganisationQueryRepositoryMock.Setup(q => q.CheckIfOrganisationHasContacts(Moq.It.IsAny<string>()))
//                 .Returns(Task.FromResult((true)));

//            ContactCreateDomainModel = Builder<ContactCreateDomainModel>.CreateNew().Build();
//            ContactQueryViewModel = Builder<Report>.CreateNew().Build();
//            ContactCreateViewModel = Builder<CreateReportRequest>.CreateNew().Build();

//            ContactRepositoryMock.Setup(q => q.CreateNewContact(Moq.It.IsAny<ContactCreateDomainModel>()))
//                        .Returns(Task.FromResult((ContactQueryViewModel)));

//            ContactRepositoryMock.Setup(q => q.CreateNewContact(Moq.It.IsAny<ContactCreateDomainModel>()))
//                .Returns(Task.FromResult(ContactQueryViewModel));

//            CreateReportHandler = new CreateReportHandler(OrganisationRepositoryMock.Object, OrganisationQueryRepositoryMock.Object, ContactRepositoryMock.Object);
//        };

//        Because of = () =>
//        {
//            _result = CreateReportHandler.Handle(ContactCreateViewModel, new CancellationToken()).Result;
//        };

//        Machine.Specifications.It verify_succesfully = () =>
//        {
//            var result = _result as Report;
//            result.Should().NotBeNull();
//        };
//    }
//}

