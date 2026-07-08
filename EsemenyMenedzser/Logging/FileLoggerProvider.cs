using System;
using Microsoft.Extensions.Logging;

namespace EsemenyMenedzser.Logging
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly string _logDirectory;
        private readonly Func<string, LogLevel, bool> _filter;
        private const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10 MB

        public FileLoggerProvider(string logDirectory, Func<string, LogLevel, bool>? filter = null)
        {
            _logDirectory = logDirectory;
            _filter = filter ?? ((category, logLevel) => true);

            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(categoryName, _logDirectory, _filter, MaxFileSizeBytes);
        }

        public void Dispose()
        {
        }
    }

    public class FileLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly string _logDirectory;
        private readonly Func<string, LogLevel, bool> _filter;
        private readonly long _maxFileSizeBytes;
        private static readonly object _lockObject = new object();

        public FileLogger(string categoryName, string logDirectory, Func<string, LogLevel, bool> filter, long maxFileSizeBytes)
        {
            _categoryName = categoryName;
            _logDirectory = logDirectory;
            _filter = filter;
            _maxFileSizeBytes = maxFileSizeBytes;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _filter(_categoryName, logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            var message = formatter(state, exception);
            var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [{logLevel}] [{_categoryName}] {message}";

            if (exception != null)
            {
                logEntry += Environment.NewLine + exception;
            }

            try
            {
                lock (_lockObject)
                {
                    var logFilePath = GetLogFilePath();
                    File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
                }
            }
            catch
            {
                // Silence any logging errors
            }
        }

        private string GetLogFilePath()
        {
            var dateString = DateTime.Now.ToString("yyyy-MM-dd");
            var baseFileName = $"application-{dateString}";
            var filePath = Path.Combine(_logDirectory, $"{baseFileName}.log");

            // Check if file exists and its size
            if (File.Exists(filePath))
            {
                var fileInfo = new FileInfo(filePath);
                if (fileInfo.Length >= _maxFileSizeBytes)
                {
                    // Find next available numbered file for today
                    filePath = GetNextRotatedLogFile(baseFileName);
                }
            }

            return filePath;
        }

        private string GetNextRotatedLogFile(string baseFileName)
        {
            int counter = 1;
            string logFilePath;

            do
            {
                logFilePath = Path.Combine(_logDirectory, $"{baseFileName}-{counter:D3}.log");
                if (!File.Exists(logFilePath))
                {
                    return logFilePath;
                }

                var fileInfo = new FileInfo(logFilePath);
                if (fileInfo.Length < _maxFileSizeBytes)
                {
                    return logFilePath;
                }

                counter++;
            }
            while (counter < 1000); // Prevent infinite loop

            // If we've created 1000 files, start overwriting the oldest
            return Path.Combine(_logDirectory, $"{baseFileName}-999.log");
        }
    }

    public static class FileLoggerExtensions
    {
        public static ILoggingBuilder AddFileLogger(this ILoggingBuilder builder, string logDirectory, Func<string, LogLevel, bool>? filter = null)
        {
            builder.AddProvider(new FileLoggerProvider(logDirectory, filter));
            return builder;
        }
    }
}
