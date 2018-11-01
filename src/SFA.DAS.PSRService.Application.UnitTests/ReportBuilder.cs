using System;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;

namespace SFA.DAS.PSRService.Application.UnitTests
{
    public static class ReportBuilder
    {
        public static Report BuildValidSubmittedReport()
        {
            var period = Period.FromInstantInPeriod(DateTime.UtcNow);

            return

                new Report
                {
                    EmployerId = "11",
                    OrganisationName = "12",
                    Id = Guid.NewGuid(),
                    ReportingPeriod = period.PeriodString,
                    Period = period,
                    Submitted = true,
                    SubmittedDetails = new Submitted
                    {
                        SubmittedAt = DateTime.UtcNow,
                        SubmittedEmail = "email",
                        SubmittedName = "SN",
                        SubmttedBy = "Dr Who",
                        UniqueReference = "BBQ"
                    },
                    ReportingPercentages = new ReportingPercentages
                    {
                        EmploymentStarts = "11",
                        TotalHeadCount = "22",
                        NewThisPeriod = "33"
                    },
                    Sections = new[]
                    {
                        new Section
                        {
                            Id = "s1",
                            Title = "t1",
                            SummaryText = "s1s",
                            Questions = new[]
                            {
                                new Question
                                {
                                    Id = "s1q1",
                                    Answer = "s1q1a",
                                    Optional = true,
                                    Type = QuestionType.LongText
                                },
                                new Question
                                {
                                    Id = "s1q2",
                                    Answer = "s1q2a",
                                    Optional = false,
                                    Type = QuestionType.ShortText
                                }
                            },
                            SubSections = null
                        },
                        new Section
                        {
                            Id = "s2",
                            Title = "t2",
                            SummaryText = "s2s",
                            Questions = null,
                            SubSections = new[]
                            {
                                new Section
                                {
                                    Id = "s2s1",
                                    Title = "s2t1",
                                    SummaryText = "s2s1s",
                                    Questions = new[]
                                    {
                                        new Question
                                        {
                                            Id = "s2s1q1",
                                            Answer = "1",
                                            Optional = true,
                                            Type = QuestionType.Number
                                        }
                                    }
                                }
                            }
                        }
                    }
                };
        }
    }
}