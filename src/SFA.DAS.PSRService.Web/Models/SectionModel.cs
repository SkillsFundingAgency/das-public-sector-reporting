using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.Web.Models;

public class SectionModel
{
    public string Id { get; set; }
    public string ReportingPeriod { get; set; }
    public IList<QuestionViewModel> Questions { get; set; }
}