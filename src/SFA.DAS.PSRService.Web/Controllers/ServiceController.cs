using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SFA.DAS.GovUK.Auth.Models;
using SFA.DAS.GovUK.Auth.Services;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Configuration.Authorization;

namespace SFA.DAS.PSRService.Web.Controllers;

[Authorize(Policy = nameof(PolicyNames.HasEmployerAccount))]
[Route("service")]
public class ServiceController(
    IWebConfiguration webConfiguration,
    IStubAuthenticationService stubAuthenticationService, 
    IConfiguration _config) : Controller
{
    public IActionResult ChangePassword(string employerId)
    {
        var baseUrl = BaseUrl();
        return Redirect($"{baseUrl}Service/Password/change");
    }

    public IActionResult ChangeEmail(string employerId)
    {
        var baseUrl = BaseUrl();
        return Redirect($"{baseUrl}Service/Email/change");
    }

    [AllowAnonymous]
    public IActionResult AccessDenied(string action)
    {
        return View("AccessDenied");
    }
    
    #if DEBUG
    [AllowAnonymous]
    [HttpGet]
    [Route("SignIn-Stub")]
    public IActionResult SigninStub()
    {
        return View("SigninStub", new List<string> { _config["StubId"], _config["StubEmail"] });
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("SignIn-Stub")]
    public async Task<IActionResult> SigninStubPost()
    {
        var claims = await stubAuthenticationService.GetStubSignInClaims(new StubAuthUserDetails
        {
            Email = _config["StubEmail"],
            Id = _config["StubId"]
        });

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claims, new AuthenticationProperties());

        return RedirectToRoute("Signed-in-stub");
    }

    [Authorize(Policy = "StubAuth")]
    [HttpGet]
    [Route("signed-in-stub", Name = "Signed-in-stub")]
    public IActionResult SignedInStub()
    {
        return View();
    }
#endif

    private string BaseUrl()
    {
        return webConfiguration.RootDomainUrl.EndsWith('/')
            ? webConfiguration.RootDomainUrl
            : $"{webConfiguration.RootDomainUrl}/";
    }
}