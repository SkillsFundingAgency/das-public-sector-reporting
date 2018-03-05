using SFA.DAS.PSRService.Api.Types.Models;
using SFA.DAS.PSRService.Domain.Exceptions;

namespace SFA.DAS.PSRService.Application.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Attributes;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;
    using Middleware;
    using Swashbuckle.AspNetCore.SwaggerGen;

    [Authorize]
    [Route("api/v1/Reports")]
    [ValidateBadRequest]
    public class ReportController : Controller
    {
        private readonly IStringLocalizer<ReportController> _localizer;
        private readonly ILogger<ReportController> _logger;
        private readonly IMediator _mediator;

        public ReportController(IMediator mediator,
            IStringLocalizer<ReportController> localizer,
            ILogger<ReportController> logger
        )
        {
            _mediator = mediator;
            _localizer = localizer;
            _logger = logger;
        }

        [HttpPost(Name = "CreateContract")]
        [SwaggerResponse((int) HttpStatusCode.Created, Type = typeof(Report))]
        [SwaggerResponse((int) HttpStatusCode.BadRequest, typeof(IDictionary<string, string>))]
        [SwaggerResponse((int) HttpStatusCode.InternalServerError, Type = typeof(ApiResponse))]
        public async Task<IActionResult> CreateReport(
            [FromBody] CreateReportRequest ReportCreateViewModel)
        {
            _logger.LogInformation("Received Create Report Request");

            var ReportQueryViewModel = await _mediator.Send(ReportCreateViewModel);

            return CreatedAtRoute("CreateContract",
                //new {ReportQueryViewModel.Username},
                ReportQueryViewModel);
        }

        [HttpPut(Name = "UpdateReport")]
        [SwaggerResponse((int) HttpStatusCode.NoContent)]
        [SwaggerResponse((int) HttpStatusCode.BadRequest, typeof(IDictionary<string, string>))]
        [SwaggerResponse((int) HttpStatusCode.InternalServerError, Type = typeof(ApiResponse))]
        public async Task<IActionResult> UpdateReport([FromBody] UpdateReportRequest ReportUpdateViewModel)
        {
            _logger.LogInformation("Received Update Report Request");

            await _mediator.Send(ReportUpdateViewModel);

            return NoContent();
        }

        [HttpDelete(Name = "Delete")]
        [SwaggerResponse((int) HttpStatusCode.NoContent)]
        [SwaggerResponse((int) HttpStatusCode.NotFound)]
        [SwaggerResponse((int) HttpStatusCode.InternalServerError, Type = typeof(ApiResponse))]
        public async Task<IActionResult> Delete(string username)
        {
            try
            {
                _logger.LogInformation("Received Delete Report Request");

                var ReportDeleteViewModel = new DeleteReportRequest
                {
                };

                await _mediator.Send(ReportDeleteViewModel);

                return NoContent();
            }
            catch (NotFound)
            {
                return NotFound();
            }
        }
    }
}