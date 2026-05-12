using System;

namespace BlobSimulation.Core.Logger {
    public struct LogEntry {
        public string Message { get; }
        public LogLevel Level { get; }
        public string Category { get; }
        public DateTime Timestamp { get; }

        public LogEntry(string message, LogLevel level = LogLevel.Info, string category = "") {
            Message = message;
            Level = level;
            Category = category;
            Timestamp = DateTime.Now;
        }
    }
}
