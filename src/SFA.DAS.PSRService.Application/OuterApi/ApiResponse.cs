using System.Net;

namespace SFA.DAS.PSRService.Application.OuterApi;

public class ApiResponse<TResponse>(TResponse body, HttpStatusCode statusCode, string errorContent)
{
    public TResponse Body { get;  } = body;
    public HttpStatusCode StatusCode { get; } = statusCode;
    public string ErrorContent { get ; } = errorContent;
}