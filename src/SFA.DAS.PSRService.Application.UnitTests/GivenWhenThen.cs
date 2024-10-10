using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SFA.DAS.PSRService.Application.UnitTests;

[ExcludeFromCodeCoverage]
public abstract class GivenWhenThen<T>
{
    protected T Sut;

    [SetUp]
    public async Task GivenWhen()
    {
        Given();
        await When();
    }

    protected virtual void Given()
    {
    }

    protected virtual Task When()
    {
        return Task.CompletedTask;
    }
}