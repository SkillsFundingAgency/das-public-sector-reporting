using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace SFA.DAS.PSRService.Web.UnitTests;

[ExcludeFromCodeCoverage]
public abstract class GivenWhenThen<T>
{
    protected T Sut;

    [SetUp]
    public void GivenWhen()
    {
        Given();
        When();
    }

    protected virtual void Given()
    {
    }

    protected virtual void When()
    {
    }
}