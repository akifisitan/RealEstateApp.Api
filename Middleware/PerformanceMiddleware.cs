using System.Diagnostics;

namespace RealEstateApp.Api.Middleware
{

    public class PerformanceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PerformanceMiddleware> _logger;

        public PerformanceMiddleware(ILogger<PerformanceMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var sw = new Stopwatch();
            sw.Start();

            await _next(httpContext);

            sw.Stop();
            _logger.LogInformation("Elapsed time for request {corrId}: {elapsed} seconds",
                httpContext.Request.Headers[TraceMiddleware.CORR_ID],
                sw.Elapsed.TotalSeconds);
        }
    }

    public static class PerformanceMiddlewareExtensions
    {
        public static IApplicationBuilder UsePerformanceMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PerformanceMiddleware>();
        }
    }
}
