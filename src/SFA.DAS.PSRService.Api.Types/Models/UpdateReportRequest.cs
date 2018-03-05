namespace SFA.DAS.PSRService.Api.Types.Models
{
    using MediatR;

    public class UpdateReportRequest : IRequest
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
    }
}
