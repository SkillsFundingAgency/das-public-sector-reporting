using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.EmployerUserAccounts.Responses;

namespace SFA.DAS.PSRService.Application.UnitTests.EmployerUserAccounts;

public class WhenMappingFromGetUserAccountsResponseToEmployerUserAccounts
{
    [Test, AutoData]
    public void Then_The_Values_Are_Mapped(GetUserAccountsResponse source)
    {
        source.IsSuspended = true;
        
        var actual = (Application.EmployerUserAccounts.EmployerUserAccounts) source;

        actual.EmployerAccounts.Should().BeEquivalentTo(source.UserAccounts);
        actual.IsSuspended.Should().Be(source.IsSuspended);
    }

    [Test]
    public void Then_If_Null_Then_Empty_Returned()
    {
        var actual = (Application.EmployerUserAccounts.EmployerUserAccounts) (GetUserAccountsResponse)null;

        actual.EmployerAccounts.Should().BeEmpty();
        actual.IsSuspended.Should().BeFalse();
    }
}