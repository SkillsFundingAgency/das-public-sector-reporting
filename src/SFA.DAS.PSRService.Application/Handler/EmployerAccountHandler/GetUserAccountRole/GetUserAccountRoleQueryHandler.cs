using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EAS.Account.Api.Client;

namespace SFA.DAS.PSRService.Application.Handler.EmployerAccountHandler.GetUserAccountRole
{
    public class GetUserAccountRoleQueryHandler : IRequestHandler<GetUserAccountRoleQuery, GetUserAccountRoleResponse>
    {
        private IAccountApiClient _client;

        public GetUserAccountRoleQueryHandler(IAccountApiClient client)
        {
            _client = client;
        }


        public async Task<GetUserAccountRoleResponse> Handle(GetUserAccountRoleQuery request, CancellationToken cancellationToken)
        {
            var accounts = await _client.GetAccountUsers(request.HashedAccountId);

            if (accounts == null || !accounts.Any())
            {
                return null;
            }

            var teamMember = accounts.FirstOrDefault(c => c.UserRef.Equals(request.UserId, StringComparison.CurrentCultureIgnoreCase));

            if (teamMember == null)
            {
                return null;
            }

            return new GetUserAccountRoleResponse(){Role = teamMember.Role};
        }
    }
}