using BlobSimulation.Core.Logger;

public class FileLogSink : ILogSink, IDisposable {
    private readonly StreamWriter writer;

    public FileLogSink(string path) {
        string? directory = Path.GetDirectoryName(path);

        if (!string.IsNullOrEmpty(directory)) {
            Directory.CreateDirectory(directory);
        }

        writer = new StreamWriter(path);
        writer.AutoFlush = true;
    }

    public void Write(LogEntry entry) {
        string categoryPart = string.IsNullOrEmpty(entry.Category) ? "" : $"[{entry.Category}] ";
        string line =
            $"[{entry.Timestamp:HH:mm:ss}] " +
            $"[{entry.Level}] " +
            $"{categoryPart}" +
            $"{entry.Message}";

        writer.WriteLine(line);
    }

    public void Dispose() {
        writer?.Dispose();
    }
}
