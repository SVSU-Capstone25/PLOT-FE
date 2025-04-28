namespace Middleware.Example;

public class TestMiddleware
{
    private readonly RequestDelegate _next;

    public TestMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    // IMessageWriter is injected into InvokeAsync
    public async Task InvokeAsync(HttpContext httpContext)
    {
        Console.WriteLine("[TestMiddleware] – entering");
        await _next(httpContext);
        Console.WriteLine("[TestMiddleware] – exiting; response headers:");
        foreach (var h in httpContext.Response.Headers)
        Console.WriteLine($"  {h.Key}: {h.Value}");
    }
}

