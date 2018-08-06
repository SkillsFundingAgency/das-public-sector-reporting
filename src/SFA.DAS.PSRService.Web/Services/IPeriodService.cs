using System;
using System.Collections.Generic;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.Web.Services
{
    public interface IPeriodService
    {
        Period GetCurrentPeriod();
        bool IsSubmissionsOpen();
    }
}