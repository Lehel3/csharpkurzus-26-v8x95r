using BlobSimulation.Core.Logger;

namespace BlobSimulation.Tests
{
    public class DummyLogger : ILogger
    {
        public List<string> Messages { get; } = new List<string>();
        public void Log(string message, LogLevel level = LogLevel.Info, string category = "", bool includeInHistory = true)
        {
            Messages.Add(message);
        }
    }
}
