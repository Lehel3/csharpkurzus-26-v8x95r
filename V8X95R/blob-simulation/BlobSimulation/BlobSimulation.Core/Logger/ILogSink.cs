using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlobSimulation.Core.Logger {

    public enum LogLevel {
        Info,
        Success,
        Warning,
        Error
    }

    public interface ILogSink {
        void Write(LogEntry entry);
    }
}
