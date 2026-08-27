using CardioTrack.ExceptionService;
using Microsoft.AspNetCore.Mvc;

namespace CardioTrack.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            _logger.LogError(ex,
             "Unhandled exception occurred. Path: {Path}, Method: {Method}, TraceId: {TraceId}",
             context.Request.Path,
             context.Request.Method,
             context.TraceIdentifier);

            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var (statusCode, title) = ex switch
        {
            Exceptions appEx => (appEx.StatusCode, GetTitleForStatus(appEx.StatusCode)),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized"),
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
            ArgumentException => (StatusCodes.Status400BadRequest, "Bad Request"),
            _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
        };

        string detail = ex is Exceptions appEx2
            ? appEx2.Message
            : "An unexpected error occurred. Please try again later.";

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path,
            Type = $"https://httpstatuses.com/{statusCode}"
        };

        problemDetails.Extensions["traceId"] = context.TraceIdentifier;

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsJsonAsync(problemDetails);
    }

    private static string GetTitleForStatus(int statusCode) => statusCode switch
    {
        400 => "Bad Request",
        401 => "Unauthorized",
        403 => "Forbidden",
        404 => "Not Found",
        409 => "Conflict",
        _ => "Error"
    };
}