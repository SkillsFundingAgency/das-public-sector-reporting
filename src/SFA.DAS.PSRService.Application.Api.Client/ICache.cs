namespace SFA.DAS.PSRService.Application.Api.Client
{
    public interface ICache
    {
        string GetString(string key);
        void SetString(string key, string value);
    }
}