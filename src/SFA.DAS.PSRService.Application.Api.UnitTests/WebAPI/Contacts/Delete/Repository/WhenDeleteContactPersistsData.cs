//namespace SFA.DAS.PSRService.Application.Api.UnitTests.WebAPI.Contacts.Put.Repository
//{
//    using FizzWare.NBuilder;
//    using FluentAssertions;
//    using Machine.Specifications;
//    using Microsoft.EntityFrameworkCore;
//    using Moq;
//    using SFA.DAS.PSRService.Data;
//    using SFA.DAS.PSRService.Domain.Entities;
//    using System.Collections.Generic;
//    using System.Threading;
//    using System.Threading.Tasks;
//    using System;
//    using System.Linq;

//    [Subject("AssessorService")]
//    public class WhenDeleteContactPersistsData
//    {
//        private static ReportRepository _contactRepository;
//        private static Mock<AssessorDbContext> _assessorDbContext;      
//        private static Mock<DbSet<Report>> _contactDBSetMock;

//        protected static Task _result;


//        Establish context = () =>
//        {
//            MappingBootstrapper.Initialize();

//            _assessorDbContext = new Mock<AssessorDbContext>();
//            _contactDBSetMock = new Mock<DbSet<Report>>();

//            var mockSet = new Mock<DbSet<Report>>();
//            var mockContext = new Mock<AssessorDbContext>();


//            var contacts = new List<Report>
//            {
//                Builder<Report>.CreateNew()              
//                .Build()
//            }.AsQueryable();

//            mockSet.As<IQueryable<Report>>().Setup(m => m.Provider).Returns(contacts.Provider);
//            mockSet.As<IQueryable<Report>>().Setup(m => m.Expression).Returns(contacts.Expression);
//            mockSet.As<IQueryable<Report>>().Setup(m => m.ElementType).Returns(contacts.ElementType);
//            mockSet.As<IQueryable<Report>>().Setup(m => m.GetEnumerator()).Returns(contacts.GetEnumerator());

//            mockContext.Setup(c => c.Contacts).Returns(mockSet.Object);

//            _assessorDbContext.Setup(q => q.Contacts).Returns(mockSet.Object);
//            _assessorDbContext.Setup(x => x.MarkAsModified(Moq.It.IsAny<Report>()));


//            _assessorDbContext.Setup(q => q.SaveChangesAsync(new CancellationToken()))
//                .Returns(Task.FromResult((Moq.It.IsAny<int>())));

//            _contactRepository = new ReportRepository(_assessorDbContext.Object);

//        };

//        Because of = () =>
//        {
//            _result = _contactRepository.Delete("1234");
//        };

//        Machine.Specifications.It verify_succesfully = () =>
//        {
//            var taskresult = _result as Task;
//            taskresult.Should().NotBeNull();
//        };
//    }
//}


