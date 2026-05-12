using BlobSimulation.Core.Logger;

public class ConsoleLogSink : ILogSink {
    public void Write(LogEntry entry) {
        Console.ForegroundColor = GetColor(entry.Level);

        string categoryPart = string.IsNullOrEmpty(entry.Category) ? "" : $"[{entry.Category}] ";

        Console.WriteLine($"{categoryPart}{entry.Message}");

        Console.ResetColor();
    }

    private ConsoleColor GetColor(LogLevel level) {
        return level switch {
            LogLevel.Success => ConsoleColor.Green,
            LogLevel.Warning => ConsoleColor.Yellow,
            LogLevel.Error => ConsoleColor.Red,
            _ => ConsoleColor.White
        };
    }
}
