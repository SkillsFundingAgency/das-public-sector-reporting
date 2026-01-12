using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.PSRService.Web.Configuration;

namespace SFA.DAS.PSRService.Web.Middleware;

public class ShutterPageMiddleware(RequestDelegate next, IWebConfiguration webConfiguration)
{
    private static readonly IRouter EmptyRouter = new ShutterPageEmptyRouter();

    public async Task InvokeAsync(HttpContext context)
    {
        if (!webConfiguration.ShutterPageEnabled)
        {
            await next(context);
            return;
        }

        if (IsHealthCheckEndpoint(context))
        {
            await next(context);
            return;
        }

        await RenderShutterPage(context);
    }

    private static bool IsHealthCheckEndpoint(HttpContext context)
    {
        return context.Request.Path.StartsWithSegments("/ping", StringComparison.OrdinalIgnoreCase);
    }

    private async Task RenderShutterPage(HttpContext context)
    {
        var razorViewEngine = context.RequestServices.GetRequiredService<IRazorViewEngine>();
        var tempDataProvider = context.RequestServices.GetRequiredService<ITempDataProvider>();
        var routeData = context.GetRouteData();
        
        var hashedAccountId = ExtractAccountId(routeData, context.Request.Path.Value);
        var homeUrl = BuildAccountHomeUrl(hashedAccountId);
        var routeDataForView = EnsureRouteDataHasAccountId(routeData, hashedAccountId);

        var actionContext = new ActionContext(context, routeDataForView, new ActionDescriptor());
        var viewResult = razorViewEngine.FindView(actionContext, "Shared/Shutter", false);

        if (!viewResult.Success || viewResult.View == null)
        {
            await next(context);
            return;
        }

        var viewData = CreateViewData(homeUrl);
        var tempData = new TempDataDictionary(context, tempDataProvider);

        await RenderViewToResponse(context, viewResult.View, routeDataForView, viewData, tempData);
    }

    private static string? ExtractAccountId(RouteData? routeData, string? path)
    {
        var accountId = routeData?.Values["hashedEmployerAccountId"]?.ToString();
        
        if (!string.IsNullOrEmpty(accountId))
        {
            return accountId;
        }

        if (string.IsNullOrEmpty(path))
        {
            return null;
        }

        var accountsIndex = path.IndexOf("/accounts/", StringComparison.OrdinalIgnoreCase);
        if (accountsIndex < 0)
        {
            return null;
        }

        var startIndex = accountsIndex + "/accounts/".Length;
        var endIndex = path.IndexOf('/', startIndex);
        
        if (endIndex > startIndex)
        {
            return path.Substring(startIndex, endIndex - startIndex);
        }
        
        if (startIndex < path.Length)
        {
            return path.Substring(startIndex);
        }

        return null;
    }

    private string BuildAccountHomeUrl(string? hashedAccountId)
    {
        if (string.IsNullOrEmpty(webConfiguration.EmployerAccountsBaseUrl))
        {
            return "";
        }

        var baseUrl = webConfiguration.EmployerAccountsBaseUrl.TrimEnd('/');
        
        return string.IsNullOrEmpty(hashedAccountId) 
            ? baseUrl 
            : $"{baseUrl}/accounts/{hashedAccountId}/teams";
    }

    private RouteData EnsureRouteDataHasAccountId(RouteData? routeData, string? hashedAccountId)
    {
        var routeDataForView = routeData ?? new RouteData();
        
        if (routeDataForView.Routers.Count == 0)
        {
            routeDataForView.Routers.Add(EmptyRouter);
        }
        
        if (!string.IsNullOrEmpty(hashedAccountId))
        {
            routeDataForView.Values.TryAdd("hashedEmployerAccountId", hashedAccountId);
        }

        return routeDataForView;
    }

    private static ViewDataDictionary CreateViewData(string homeUrl)
    {
        return new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
        {
            ["HomeUrl"] = homeUrl
        };
    }

    private static async Task RenderViewToResponse(
        HttpContext context,
        Microsoft.AspNetCore.Mvc.ViewEngines.IView view,
        RouteData routeData,
        ViewDataDictionary viewData,
        TempDataDictionary tempData)
    {
        await using var writer = new StringWriter();
        var actionContext = new ActionContext(context, routeData, new ActionDescriptor());
        var viewContext = new ViewContext(
            actionContext,
            view,
            viewData,
            tempData,
            writer,
            new HtmlHelperOptions());

        await view.RenderAsync(viewContext);

        context.Response.StatusCode = 200;
        context.Response.ContentType = "text/html; charset=utf-8";
        await context.Response.WriteAsync(writer.ToString());
    }

    private class ShutterPageEmptyRouter : IRouter
    {
        public VirtualPathData? GetVirtualPath(VirtualPathContext context)
        {
            return null;
        }

        public Task RouteAsync(RouteContext context)
        {
            return Task.CompletedTask;
        }
    }
}
