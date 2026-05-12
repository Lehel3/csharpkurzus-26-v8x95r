namespace BlobSimulation.Core.Logger {
    public interface ILogger {
        void Log(string message, LogLevel level = LogLevel.Info, string category = "", bool includeInHistory = true);

        void Warn(string message, string category = "", bool includeInHistory = true) {
            Log(message, LogLevel.Warning, category, includeInHistory);
        }

        void Error(string message, string category = "", bool includeInHistory = true) {
            Log(message, LogLevel.Error, category, includeInHistory);
        }
    }
}

