using AutoMapper;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Messages.Events;

namespace SFA.DAS.PSRService.Application.Mapping
{
    public class ReportSubmittedProfile : Profile
    {
        public ReportSubmittedProfile()
        {
            CreateMap<Report, ReportSubmitted>()
                .ConvertUsing(report => new ReportSubmitted
                {
                    Id = report.Id,
                    EmployerId = report.EmployerId,
                    ReportingPeriod = report.Period.PeriodString,
                    Submitter = new Submitter
                    {
                        Email = report.SubmittedDetails.SubmittedEmail,
                        Name = report.SubmittedDetails.SubmittedName,
                        UserId = report.SubmittedDetails.SubmttedBy
                    },
                    ReportingPercentages = new Messages.Events.ReportingPercentages
                    {
                        EmploymentStarts = report.ReportingPercentages.EmploymentStarts,
                        TotalHeadCount = report.ReportingPercentages.TotalHeadCount,
                        NewThisPeriod = report.ReportingPercentages.NewThisPeriod
                    },
                    Answers = new Answers
                    {
                        OrganisationName = report.OrganisationName,
                        YourEmployees = new YourEmployees
                        {
                            AtStart = report.Answers.YourEmployees.AtStart,
                            AtEnd = report.Answers.YourEmployees.AtEnd,
                            NewThisPeriod = report.Answers.YourEmployees.NewThisPeriod
                        },
                        YourApprentices = new YourApprentices
                        {
                            AtStart = report.Answers.YourApprentices.AtStart,
                            AtEnd = report.Answers.YourApprentices.AtEnd,
                            NewThisPeriod = report.Answers.YourApprentices.NewThisPeriod
                        },
                        FullTimeEquivalents = report.Answers.FullTimeEquivalents,
                        OutlineActions = report.Answers.OutlineActions,
                        Challenges = report.Answers.Challenges,
                        TargetPlans = report.Answers.TargetPlans,
                        AnythingElse = report.Answers.AnythingElse
                    }
                });
        }
    }
}
