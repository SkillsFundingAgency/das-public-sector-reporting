using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Microsoft.AspNetCore.Mvc
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
        /// <param name="actionName">The name of the action method.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns>The absolute URL.</returns>
        public static string EasAction(
            this IUrlHelper url
            , string path, string EasBaseUrl)
        {
            var hashedAccountId = url.ActionContext.RouteData.Values["employerAccountId"];

                  return $"{EasBaseUrl}/accounts/{hashedAccountId}/{path}";
        }

        public static string ExternalUrlAction(this IUrlHelper helper, string baseUrl, string controllerName, string actionName = "", bool ignoreAccountId = false)
        {
            var accountId = helper.ActionContext.RouteData.Values["employerAccountId"];
            
            baseUrl = baseUrl.EndsWith("/") ? baseUrl : baseUrl + "/";

            return ignoreAccountId ? $"{baseUrl}{controllerName}/{actionName}"
                  : $"{baseUrl}accounts/{accountId}/{controllerName}/{actionName}";
        }
    }
}