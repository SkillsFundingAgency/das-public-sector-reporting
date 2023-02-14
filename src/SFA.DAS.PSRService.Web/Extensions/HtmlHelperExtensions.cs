using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.MA.Shared.UI.Configuration;
using SFA.DAS.MA.Shared.UI.Models;
using SFA.DAS.MA.Shared.UI.Models.Links;
using SFA.DAS.PSRService.Web.Configuration;

namespace SFA.DAS.PSRService.Web.Extensions
{
    /// <summary>
    /// <see cref="IHtmlHelper"/> extension methods.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        public static IHeaderViewModel GetHeaderViewModel(this IHtmlHelper html, bool hideMenu = false, bool useLegacyStyles = false)
        {
            var configuration = html.ViewContext.HttpContext.RequestServices.GetService(typeof(IWebConfiguration)) as IWebConfiguration;
            var urlHelperFactory = (IUrlHelperFactory)html.ViewContext.HttpContext.RequestServices.GetService(typeof(IUrlHelperFactory));
            var urlHelper = urlHelperFactory.GetUrlHelper(html.ViewContext);

            var headerModel = new HeaderViewModel(new HeaderConfiguration
            {
                EmployerCommitmentsV2BaseUrl = configuration.EmployerCommitmentsV2BaseUrl,
                EmployerFinanceBaseUrl = configuration.RootDomainUrl,
                ManageApprenticeshipsBaseUrl = configuration.RootDomainUrl,
                AuthenticationAuthorityUrl = configuration.Identity.Authority,
                ClientId = configuration.Identity.ClientId,
                EmployerRecruitBaseUrl = configuration.EmployerRecruitBaseUrl,
                SignOutUrl = new System.Uri(configuration.ApplicationUrl + urlHelper.Action("Logout", "Home")),
                ChangeEmailReturnUrl = new System.Uri(configuration.ApplicationUrl + "/service/changeEmail"),
                ChangePasswordReturnUrl = new System.Uri(configuration.ApplicationUrl + "/service/changePassword"),                
            },
            new UserContext
            {
                User = html.ViewContext.HttpContext.User,
                HashedAccountId = html.ViewContext.RouteData.Values["hashedEmployerAccountId"]?.ToString()
            },
            useLegacyStyles : useLegacyStyles
            );

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

        public static IFooterViewModel GetFooterViewModel(this IHtmlHelper html, bool useLegacyStyles = false)
        {
            var configuration = html.ViewContext.HttpContext.RequestServices.GetService(typeof(IWebConfiguration)) as IWebConfiguration;

            return new FooterViewModel(new FooterConfiguration
            {
                ManageApprenticeshipsBaseUrl = configuration.EmployerAccountsBaseUrl
            },
            new UserContext
            {
                User = html.ViewContext.HttpContext.User,
                HashedAccountId = html.ViewContext.RouteData.Values["hashedEmployerAccountId"]?.ToString()
            },
            useLegacyStyles : useLegacyStyles);
        }

        public static ICookieBannerViewModel GetCookieBannerViewModel(this IHtmlHelper html)
        {
            var configuration = html.ViewContext.HttpContext.RequestServices.GetService(typeof(IWebConfiguration)) as IWebConfiguration;

            return new CookieBannerViewModel(new CookieBannerConfiguration
            {
                ManageApprenticeshipsBaseUrl = configuration.EmployerAccountsBaseUrl
            },
            new UserContext
            {
                User = html.ViewContext.HttpContext.User,
                HashedAccountId = html.ViewContext.RouteData.Values["hashedEmployerAccountId"]?.ToString()
            }                       
            );
        }

        public static string GetZenDeskSnippetKey(this IHtmlHelper html)
        {
            var configuration = html.ViewContext.HttpContext.RequestServices.GetService(typeof(IWebConfiguration)) as IWebConfiguration;
            return configuration.ZenDeskConfig.SnippetKey;
        }

        public static string GetZenDeskSnippetSectionId(this IHtmlHelper html)
        {
            var configuration = html.ViewContext.HttpContext.RequestServices.GetService(typeof(IWebConfiguration)) as IWebConfiguration;
            return configuration.ZenDeskConfig.SectionId;
        }

        public static string GetZenDeskCobrowsingSnippetKey(this IHtmlHelper html)
        {
            var configuration = html.ViewContext.HttpContext.RequestServices.GetService(typeof(IWebConfiguration)) as IWebConfiguration;
            return configuration.ZenDeskConfig.CobrowsingSnippetKey;
        }

        public static HtmlString SetZenDeskLabels(this IHtmlHelper html, params string[] labels)
        {
            var keywords = string.Join(",", labels
                .Where(label => !string.IsNullOrEmpty(label))
                .Select(label => $"'{EscapeApostrophes(label)}'"));

            // when there are no keywords default to empty string to prevent zen desk matching articles from the url
            var apiCallString = "zE('webWidget', 'helpCenter:setSuggestions', { labels: ["
                                + (!string.IsNullOrEmpty(keywords) ? keywords : "''")
                                + "] });";

            return new HtmlString(apiCallString);
        }

        private static string EscapeApostrophes(string input)
        {
            return input.Replace("'", @"\'");
        }
    }
}