using System.Collections.Generic;
using System.Linq;
using SFA.DAS.PSRService.Application.EmployerUserAccounts.Responses;

namespace SFA.DAS.PSRService.Application.EmployerUserAccounts;

public class EmployerUserAccounts
{
    public IEnumerable<EmployerUserAccountItem> EmployerAccounts { get ; set ; }
    public bool IsSuspended { get; set; }
    
    public string EmployerUserId { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }
    public static implicit operator EmployerUserAccounts(GetUserAccountsResponse source)
    {
        var accounts = source?.UserAccounts == null
            ? []
            : source.UserAccounts.Select(c => (EmployerUserAccountItem) c).ToList();
        
        return new EmployerUserAccounts
        {
            EmployerAccounts = accounts,
            IsSuspended = source?.IsSuspended ?? false,
            FirstName = source?.FirstName ?? "",
            LastName = source?.LastName ?? "",
            EmployerUserId = source?.EmployerUserId ?? "",
        };
    }

}

public class EmployerUserAccountItem
{
    public string AccountId { get; set; }
    public string EmployerName { get; set; }
    public string Role { get; set; }
        
    public static implicit operator EmployerUserAccountItem(EmployerIdentifier source)
    {
        return new EmployerUserAccountItem
        {
            AccountId = source.AccountId,
            EmployerName = source.EmployerName,
            Role = source.Role
        };
    }
}