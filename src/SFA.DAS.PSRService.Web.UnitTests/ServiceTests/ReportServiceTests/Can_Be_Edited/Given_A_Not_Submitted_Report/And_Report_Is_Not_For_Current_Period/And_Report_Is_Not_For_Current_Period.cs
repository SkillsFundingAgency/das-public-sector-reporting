﻿using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.ReportServiceTests.Can_Be_Edited.Given_A_Not_Submitted_Report.And_Report_Is_Not_For_Current_Period;

[ExcludeFromCodeCoverage]
public abstract class And_Report_Is_Not_For_Current_Period : Given_A_Not_Submitted_Report
{
    protected And_Report_Is_Not_For_Current_Period()
    {
        MockPeriodService.Setup(m => m.PeriodIsCurrent(It.IsAny<Period>()))
            .Returns(false);
    }
}