﻿using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.ViewModels;

public class PercentagesViewModel
{
    public string EmploymentStarts { get; }
    public string TotalHeadCount { get; }
    public string NewThisPeriod { get; }
    public string Title { get; }
    
    public PercentagesViewModel(ReportingPercentages percentages)
    {
        if (percentages == null)
        {
            return;
        }

        EmploymentStarts = percentages.EmploymentStarts + "%";
        TotalHeadCount = percentages.TotalHeadCount + "%";
        NewThisPeriod = percentages.NewThisPeriod + "%";
        Title = percentages.Title;
    }
}