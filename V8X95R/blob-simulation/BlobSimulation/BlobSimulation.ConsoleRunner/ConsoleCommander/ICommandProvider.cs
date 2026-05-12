using BlobSimulation.ConsoleRunner.ConsoleCommander.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlobSimulation.ConsoleRunner.ConsoleCommander {

    public interface ICommandProvider {

        IReadOnlyDictionary<string, ConsoleCommand> Commands { get; }

    }
}
