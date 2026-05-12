using BlobSimulation.Core.Logger;

namespace BlobSimulation.ConsoleRunner.ConsoleCommander.Commands {

    [CommandName("load")]
    public sealed class LoadCommand : ConsoleCommand {
        public string Path { get; }
        public LoadCommand() : this(string.Empty) { }

        public LoadCommand(string path) {
            Path = path;
        }


        public override CommandParseResult Parse(string[] parts) {
            string rawInput = string.Join(" ", parts);
            if (parts.Length < 2)
                return CommandParseResult.Error(rawInput, "Load command requires a path.");

            return CommandParseResult.Ok(new LoadCommand(parts[1]), rawInput);
        }

        public override bool Execute(CommandContext context) {
            context.Simulation.LoadSimulation(Path);
            context.Logger.Log($"Loaded simulation from {Path}.", LogLevel.Success, "Load");
            return false;
        }
    }
}
