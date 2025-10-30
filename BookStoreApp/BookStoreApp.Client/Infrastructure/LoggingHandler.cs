namespace BookStoreApp.Client.Infrastructure;

public class LoggingHandler : DelegatingHandler
{
    private readonly ILogger<LoggingHandler> _logger;

    public LoggingHandler(ILogger<LoggingHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("HTTP {Method} {Url}", request.Method, request.RequestUri);

        if (request.Content != null)
        {
            var body = await request.Content.ReadAsStringAsync();
            _logger.LogInformation("Request Body: {Body}", body);
        }

        var response = await base.SendAsync(request, cancellationToken);

        _logger.LogInformation("Response Status: {StatusCode}", response.StatusCode);

        return response;
    }
}

