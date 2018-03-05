//namespace SFA.DAS.PSRService.Application.Api.UnitTests.WebAPI.ContactContoller.Put.Repository
//{
//    using FizzWare.NBuilder;
//    using Machine.Specifications;
//    using Microsoft.EntityFrameworkCore;
//    using Moq;
//    using SFA.DAS.PSRService.Data;
//    using SFA.DAS.PSRService.Domain.Entities;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Threading;
//    using System.Threading.Tasks;

//    [Subject("AssessorService")]
//    public class WhenUpdateContactPersistsData
//    {
//        private static ReportRepository _reportRepository;

//        protected static Contact _result;


//        Establish context = () =>
//        {
//            MappingBootstrapper.Initialize();

//            _contactUpdateViewModel = Builder<UpdateContactRequest>.CreateNew().Build();

//            _assessorDbContext = new Mock<AssessorDbContext>();
//            _contactDBSetMock = new Mock<DbSet<AssessorService.Domain.Entities.Contact>>();

//            var mockSet = new Mock<DbSet<AssessorService.Domain.Entities.Contact>>();
//            var mockContext = new Mock<AssessorDbContext>();


//            var contacts = new List<AssessorService.Domain.Entities.Contact>
//            {
//                Builder<AssessorService.Domain.Entities.Contact>.CreateNew().Build()
//            }.AsQueryable();

//            mockSet.As<IQueryable<AssessorService.Domain.Entities.Contact>>().Setup(m => m.Provider).Returns(contacts.Provider);
//            mockSet.As<IQueryable<AssessorService.Domain.Entities.Contact>>().Setup(m => m.Expression).Returns(contacts.Expression);
//            mockSet.As<IQueryable<AssessorService.Domain.Entities.Contact>>().Setup(m => m.ElementType).Returns(contacts.ElementType);
//            mockSet.As<IQueryable< AssessorService.Domain.Entities.Contact>>().Setup(m => m.GetEnumerator()).Returns(contacts.GetEnumerator());

//            mockContext.Setup(c => c.Contacts).Returns(mockSet.Object);

//            _assessorDbContext.Setup(q => q.Contacts).Returns(mockSet.Object);
//            _assessorDbContext.Setup(x => x.MarkAsModified(Moq.It.IsAny< AssessorService.Domain.Entities.Contact>()));


//            _assessorDbContext.Setup(q => q.SaveChangesAsync(new CancellationToken()))
//                .Returns(Task.FromResult((Moq.It.IsAny<int>())));

//            _reportRepository = new ReportRepository(_assessorDbContext.Object);
//        };

//        Because of = () =>
//        {
//            _reportRepository.Update(_contactUpdateViewModel);
//        };

//        Machine.Specifications.It verify_succesfully = () =>
//        {          
//        };
//    }
//}

