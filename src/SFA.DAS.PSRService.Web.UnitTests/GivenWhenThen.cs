namespace SFA.DAS.PSRService.Web.UnitTests;

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

    protected virtual async Task When()
    {
    }
}