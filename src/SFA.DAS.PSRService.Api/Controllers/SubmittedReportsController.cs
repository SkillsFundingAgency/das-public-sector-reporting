using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MediatR;
using SFA.DAS.PSRService.Api.Attributes;

namespace SFA.DAS.PSRService.Api.Controllers
{
    [ApiAuthorize(Roles = "ReadUserAccounts")]
    [RoutePrefix("api/submittedreports")]
    public class SubmittedReportsController : ApiController
    {
        private IMediator _mediator;

        public SubmittedReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("")]
        public void GetSubmittedReports()
        {
            // TODO: repository, queryhandler etc + add the stored procs in
        }
    }
}
