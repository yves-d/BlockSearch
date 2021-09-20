using Microsoft.Extensions.Logging;

namespace BlockSearch.Infrastructure.Logger
{
    // I can't claim this as my own, but I found it very useful for testing
    // source: https://chrissainty.com/unit-testing-ilogger-in-aspnet-core/
    public class LoggerAdapter<T> : ILoggerAdapter<T>
    {
        private readonly ILogger<T> _logger;

        public LoggerAdapter(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void LogInformation(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }

        public void LogError(string message, params object[] args)
        {
            _logger.LogError(message, args);
        }
    }
}
