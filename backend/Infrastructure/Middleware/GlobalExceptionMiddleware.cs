using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace backend.Infrastructure.Middleware;

public sealed class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await WriteProblemDetailsAsync(context, ex);
        }
    }
    private async Task WriteProblemDetailsAsync(HttpContext context, Exception ex)
    {
        var (status, title, detail, type) = MapProblem(ex);
        var problem = new ProblemDetails
        {
            Type = type,
            Title = title,
            Status = (int)status,
            Detail = detail,
            Instance = context.Request.Path
        };

        // Attach trace id for correlation
        var traceId = context.TraceIdentifier;
        problem.Extensions["traceId"] = traceId;

        // Log with structured data
        _logger.LogError(ex, "Unhandled exception {Title}. Status {Status}. TraceId {TraceId}", title, (int)status, traceId);
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = problem.Status ?? (int)HttpStatusCode.InternalServerError;
        var json = JsonSerializer.Serialize(problem, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        await context.Response.WriteAsync(json);
    }
    private static (HttpStatusCode status, string title, string detail, string type) MapProblem(Exception ex)
    {
        return ex switch
        {
            ArgumentNullException ane => (HttpStatusCode.BadRequest, "Invalid argument", ane.Message, "https://httpstatuses.com/400"),
            ArgumentException ae => (HttpStatusCode.BadRequest, "Invalid argument", ae.Message, "https://httpstatuses.com/400"),
            KeyNotFoundException knf => (HttpStatusCode.NotFound, "Resource not found", knf.Message, "https://httpstatuses.com/404"),
            UnauthorizedAccessException ua => (HttpStatusCode.Unauthorized, "Unauthorized", ua.Message, "https://httpstatuses.com/401"),
            InvalidOperationException ioe => (HttpStatusCode.Conflict, "Operation invalid", ioe.Message, "https://httpstatuses.com/409"),
            DbUpdateConcurrencyException dce => (HttpStatusCode.Conflict, "Concurrency conflict", "The resource was updated by another process.", "https://httpstatuses.com/409"),
            DbUpdateException due => (HttpStatusCode.BadRequest, "Database update failed", GetInnermostMessage(due), "https://httpstatuses.com/400"),
            NotImplementedException nie => (HttpStatusCode.NotImplemented, "Not implemented", "The requested operation is not implemented.", "https://httpstatuses.com/501"),
            TimeoutException te => (HttpStatusCode.RequestTimeout, "Request timeout", te.Message, "https://httpstatuses.com/408"),
            _ => (HttpStatusCode.InternalServerError, "Unexpected error", "An unexpected error occurred. Please try again later.", "https://httpstatuses.com/500")
        };
    }
    private static string GetInnermostMessage(Exception ex)
    {
        while (ex.InnerException is not null) ex = ex.InnerException;
        return ex.Message;
    }
}

public static class GlobalExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionMiddleware>();
    }
}