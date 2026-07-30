namespace MyFirstApi.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate request;
        public RequestLoggingMiddleware(RequestDelegate request) // Dependency Injection
        {
            this.request = request;
        }
        //  logs each request's method and path to the console
        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine($"Request : {context.Request.Method} {context.Request.Path}");
            await request(context);
        }
    }
}
