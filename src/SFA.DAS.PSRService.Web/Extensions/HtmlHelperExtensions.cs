using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using SFA.DAS.PSRService.Web.Configuration;

namespace SFA.DAS.PSRService.Web.Extensions
{
    /// <summary>
    /// <see cref="IHtmlHelper"/> extension methods.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        

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