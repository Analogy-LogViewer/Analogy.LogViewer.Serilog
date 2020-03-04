using Analogy.Interfaces;
using System;

namespace Analogy.LogViewer.Serilog.Managers
{
    public class LogManager : IAnalogyLogger
    {
        private static Lazy<LogManager> _instance = new Lazy<LogManager>();
        public static LogManager Instance => _instance.Value;
        private IAnalogyLogger Logger { get; set; }

        public void SetLogger(IAnalogyLogger logger) => Logger = logger;

        public void LogEvent(string source, string message, string memberName = "", int lineNumber = 0,
            string filePath = "")
        {
            Logger.LogEvent(source, message, memberName, lineNumber, filePath);
        }

        public void LogWarning(string source, string message, string memberName = "", int lineNumber = 0,
            string filePath = "")
        {
            Logger.LogWarning(source, message, memberName, lineNumber, filePath);
        }

        public void LogDebug(string source, string message, string memberName = "", int lineNumber = 0,
            string filePath = "")
        {
            Logger.LogDebug(source, message, memberName, lineNumber, filePath);
        }

        public void LogError(string source, string message, string memberName = "", int lineNumber = 0,
            string filePath = "")
        {
            Logger.LogError(source, message, memberName, lineNumber, filePath);
        }

        public void LogCritical(string source, string message, string memberName = "", int lineNumber = 0,
            string filePath = "")
        {
            Logger.LogCritical(source, message, memberName, lineNumber, filePath);
        }

        public void LogException(Exception ex, string source, string message, string memberName = "",
            int lineNumber = 0,
            string filePath = "")
        {
            Logger.LogException(ex, source, message, memberName, lineNumber, filePath);
        }
    }
}
