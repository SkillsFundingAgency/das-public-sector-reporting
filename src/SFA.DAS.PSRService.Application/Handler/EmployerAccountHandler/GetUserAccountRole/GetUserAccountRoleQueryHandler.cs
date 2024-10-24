using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EAS.Account.Api.Client;

namespace SFA.DAS.PSRService.Application.Handler.EmployerAccountHandler.GetUserAccountRole;

public class GetUserAccountRoleQueryHandler(IAccountApiClient client) : IRequestHandler<GetUserAccountRoleQuery, GetUserAccountRoleResponse>
{
    public async Task<GetUserAccountRoleResponse> Handle(GetUserAccountRoleQuery request, CancellationToken cancellationToken)
    {
        var accounts = await client.GetAccountUsers(request.HashedAccountId);

        if (accounts == null || accounts.Count == 0)
        {
            return null;
        }

        var teamMember = accounts.FirstOrDefault(c => c.UserRef.Equals(request.UserId, StringComparison.CurrentCultureIgnoreCase));

        return teamMember == null ? null : new GetUserAccountRoleResponse {Role = teamMember.Role};
    }
}