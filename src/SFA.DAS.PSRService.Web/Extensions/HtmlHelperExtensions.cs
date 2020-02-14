using SFA.DAS.MA.Shared.UI.Models;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.MA.Shared.UI.Configuration;
using SFA.DAS.Authorization.Services;
using SFA.DAS.MA.Shared.UI.Models.Links;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Microsoft.AspNetCore.Mvc
{
    /// <summary>
    /// <see cref="IHtmlHelper"/> extension methods.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        public static IHeaderViewModel GetHeaderViewModel(this IHtmlHelper html, bool hideMenu = false)
        {
            var configuration = html.ViewContext.HttpContext.RequestServices.GetService(typeof(IWebConfiguration)) as IWebConfiguration;
            var authorizationService = html.ViewContext.HttpContext.RequestServices.GetService(typeof(IAuthorizationService)) as IAuthorizationService;
            var urlHelperFactory = (IUrlHelperFactory)html.ViewContext.HttpContext.RequestServices.GetService(typeof(IUrlHelperFactory));
            var urlHelper = urlHelperFactory.GetUrlHelper(html.ViewContext);

            var headerModel = new HeaderViewModel(new HeaderConfiguration
            {
                EmployerCommitmentsBaseUrl = new System.Uri(configuration.HomeUrl).AbsoluteUri.Replace(new System.Uri(configuration.HomeUrl).AbsolutePath, ""),
                EmployerFinanceBaseUrl = configuration.RootDomainUrl,
                ManageApprenticeshipsBaseUrl = configuration.RootDomainUrl,
                AuthenticationAuthorityUrl = configuration.Identity.Authority,
                ClientId = configuration.Identity.ClientId,
                EmployerRecruitBaseUrl = configuration.RootDomainUrl,
                AuthorizationService = authorizationService,
                SignOutUrl = new System.Uri(configuration.ApplicationUrl + urlHelper.Action("Logout", "Home")),
                ChangeEmailReturnUrl = new System.Uri(configuration.ApplicationUrl + "/service/changeEmail"),
                ChangePasswordReturnUrl = new System.Uri(configuration.ApplicationUrl + "/service/changePassword")
            },
            new UserContext
            {
                User = html.ViewContext.HttpContext.User,
                HashedAccountId = html.ViewContext.RouteData.Values["employerAccountId"]?.ToString()
            });

            headerModel.SelectMenu(html.ViewBag.Section);

            if (html.ViewBag.HideNav != null && html.ViewBag.HideNav || hideMenu)
            {
                headerModel.HideMenu();
            }

            if (html.ViewData.Model?.GetType().GetProperty("HideHeaderSignInLink") != null)
            {
                headerModel.RemoveLink<SignIn>();
            }

            return headerModel;
        }

        public static IFooterViewModel GetFooterViewModel(this IHtmlHelper html)
        {
            var configuration = html.ViewContext.HttpContext.RequestServices.GetService(typeof(IWebConfiguration)) as IWebConfiguration;

            return new FooterViewModel(new FooterConfiguration
            {
                ManageApprenticeshipsBaseUrl = configuration.RootDomainUrl
            },
            new UserContext
            {
                User = html.ViewContext.HttpContext.User,
                HashedAccountId = html.ViewContext.RouteData.Values["employerAccountId"]?.ToString()
            });
        }
    }
}