using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PSRService.Web.Configuration;

namespace SFA.DAS.PSRService.Web.Extensions
{
    /// <summary>
    /// <see cref="IUrlHelper"/> extension methods.
    /// </summary>
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Generates a fully qualified URL to an action method by using the specified action name, controller name and
        /// route values.
        /// </summary>
        /// <param name="url">The URL helper.</param>
        /// <param name="path"></param>
        /// <param name="easBaseUrl"></param>
        /// <returns>The absolute URL.</returns>
        public static string EasAction(
            this IUrlHelper url
            , string path, string easBaseUrl)
        {
            var hashedAccountId = url.ActionContext.RouteData.Values["employerAccountId"];

                  return $"{easBaseUrl}/accounts/{hashedAccountId}/{path}";
        }
        
        public static string ExternalUrlAction(this IUrlHelper helper, string baseUrl, string controllerName, string actionName = "", bool ignoreAccountId = false)
        {
            var accountId = helper.ActionContext.RouteData.Values["employerAccountId"];
            
            baseUrl = baseUrl.EndsWith("/") ? baseUrl : baseUrl + "/";

            return ignoreAccountId ? $"{baseUrl}{controllerName}/{actionName}"
                : $"{baseUrl}accounts/{accountId}/{controllerName}/{actionName}";
        }

        public static string AccountsAction(this IUrlHelper helper, string controller, string action, bool includedAccountId = true)
        {
            var configuration = DependencyResolver.Current.GetService<WebConfiguration>();
            var baseUrl = configuration.HomeUrl;
            if (includedAccountId)
            {
                var hashedAccountId = helper.ActionContext.RouteData.Values["employerAccountId"]; 
                return Action(baseUrl, controller, action, hashedAccountId?.ToString());
            }
            return Action(baseUrl, controller, action);
        }

        private static string Action(string baseUrl, string controller, string action, string hashedAccountId)
        {
            var trimmedBaseUrl = baseUrl.TrimEnd('/');

            return $"{trimmedBaseUrl}/{controller.TrimEnd('/')}/{hashedAccountId}/{action}".TrimEnd('/');
        }

        private static string Action(string baseUrl, string controller, string action)
        {
            var trimmedBaseUrl = baseUrl.TrimEnd('/');

            return $"{trimmedBaseUrl}/{controller.TrimEnd('/')}/{action}".TrimEnd('/');
        }

        private static string Action(UrlHelper helper, string baseUrl, string path)
        {
            var hashedAccountId = helper.RequestContext.RouteData.Values["employerAccountId"];
            var accountPath = hashedAccountId == null ? $"{path}" : $"{hashedAccountId}/{path}";

            return Action(baseUrl, accountPath);
        }
    }
}