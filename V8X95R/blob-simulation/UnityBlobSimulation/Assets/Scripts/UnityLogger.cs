using UnityEngine;
using BlobSimulation.Core;
using BlobSimulation.Core.Logger;
using ILogger = BlobSimulation.Core.Logger.ILogger;

public class UnityLogger : ILogger
{

    public void Log(string message, LogLevel level = LogLevel.Info, string category = "", bool includeInHistory = true)
    {
        //remove debug bloat, keep in logs
        if (!includeInHistory) return;

        string formattedMessage = string.IsNullOrWhiteSpace(category) ? message : $"[{category}] {message}";

        switch (level) {
            case LogLevel.Warning:
                Debug.LogWarning(formattedMessage);
                break;

            case LogLevel.Error:
                Debug.LogError(formattedMessage);
                break;

            default:
                Debug.Log(formattedMessage);
                break;
        }
    }

}
