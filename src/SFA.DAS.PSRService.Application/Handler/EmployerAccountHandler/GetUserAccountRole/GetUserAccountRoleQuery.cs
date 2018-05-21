using MediatR;

namespace SFA.DAS.PSRService.Application.Handler.EmployerAccountHandler.GetUserAccountRole
{
    public class GetUserAccountRoleQuery : IRequest<GetUserAccountRoleResponse>
    {
        public GetUserAccountRoleQuery(string hashedAccountId, string userId)
        {
            if (string.IsNullOrWhiteSpace(hashedAccountId))
            {
                throw new System.ArgumentException("hashedAccountId cannot be null or empty", nameof(hashedAccountId));
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new System.ArgumentException("userId cannot be null or empty", nameof(userId));
            }

            HashedAccountId = hashedAccountId;
            UserId = userId;
        }

        public string HashedAccountId { get; }

        public string UserId { get; }
    }
}
