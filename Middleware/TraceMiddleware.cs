namespace RealEstateApp.Api.Middleware
{
    public class TraceMiddleware
    {
        public const string CORR_ID = "CorrId";
        private readonly RequestDelegate _next;
        private readonly ILogger<TraceMiddleware> _logger;

        public TraceMiddleware(ILogger<TraceMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (!httpContext.Request.Headers.ContainsKey(CORR_ID))
            {
                var corrId = Guid.NewGuid().ToString();
                httpContext.Request.Headers.Add(CORR_ID, corrId);
                _logger.LogInformation("-------------- New Request Received -------------- CORRID = {corrId}", corrId);
            }
            _logger.LogInformation("Request method: {method}, path: {path}, CorrId: {corrId}, started at: {datetime}", httpContext.Request.Method, httpContext.Request.Path, httpContext.Request.Headers[TraceMiddleware.CORR_ID], DateTime.Now);
            await _next(httpContext);
        }
    }

    public static class TraceMiddlewareMiddlewareExtensions
    {
        public static IApplicationBuilder UseTraceMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TraceMiddleware>();
        }
    }
}
