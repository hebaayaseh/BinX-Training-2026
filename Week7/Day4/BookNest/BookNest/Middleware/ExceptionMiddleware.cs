using BookNest.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace BookNest.Middleware
{
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
                    "Unhandled exception occurred. Path: {Path}, Method: {Method}",
                    context.Request.Path,
                    context.Request.Method);

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var (statusCode, title) = ex switch
            {
                AppException appEx => (appEx.StatusCode, appEx.GetType().Name),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized"),
                _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
            };

            string detail = ex is AppException
                ? ex.Message
                : "An unexpected error occurred. Please try again later.";

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = context.Request.Path
            };
            problemDetails.Extensions["traceId"] = context.TraceIdentifier;

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}