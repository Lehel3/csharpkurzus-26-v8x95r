using BlobSimulation.Core.Logger;

public enum LogMode {
    Instant,
    Buffered
}

public class ConsoleLogger: ILogger {
    private readonly FileLogSink fileSink;
    private readonly ConsoleLogSink consoleSink;
    private readonly List<LogEntry> buffer = new List<LogEntry>();

    public LogMode Mode { get; set; } = LogMode.Instant;

    public ConsoleLogger(FileLogSink fileSink, ConsoleLogSink consoleSink) {
        this.fileSink = fileSink;
        this.consoleSink = consoleSink;
    }

    public void Log(string message, LogLevel level = LogLevel.Info, string category = "System", bool includeInHistory = true) {
        var entry = new LogEntry(message, level, category);

        // Always persist immediately to file.
        if (fileSink != null)
            fileSink.Write(entry);

        // Silent logs do not go to console/history view.
        if (!includeInHistory)
            return;

        // Console behavior depends on mode.
        if (Mode == LogMode.Instant) {
            if (consoleSink != null)
                consoleSink.Write(entry);
        }
        else {
            buffer.Add(entry);
        }
    }

    public List<LogEntry> GetBufferedLogs() {
        return buffer;
    }

    public void Dump() {
        foreach (var entry in buffer)
            consoleSink.Write(entry);

        buffer.Clear();
    }

    public void ClearBuffer() {
        buffer.Clear();
    }
}
