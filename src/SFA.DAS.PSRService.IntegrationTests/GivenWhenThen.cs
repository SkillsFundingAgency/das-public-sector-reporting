namespace SFA.DAS.PSRService.IntegrationTests;

[ExcludeFromCodeCoverage]
public abstract class GivenWhenThen<TypeOfSUT>
{
    protected TypeOfSUT Sut;

    [SetUp]
    public async Task GivenWhen()
    {
        await Given();
        await When();
    }

    protected virtual async Task Given()
    {
    }

    protected virtual async Task When()
    {
    }
}