namespace SFA.DAS.PSRService.IntegrationTests;

[ExcludeFromCodeCoverage]
public abstract class GivenWhenThen<TSut>
{
    protected TSut Sut;

    [SetUp]
    public async Task GivenWhen()
    {
        await Given();
        await When();
    }

    protected virtual Task Given()
    {
        return Task.CompletedTask;
    }

    protected virtual Task When()
    {
        return Task.CompletedTask;
    }
}