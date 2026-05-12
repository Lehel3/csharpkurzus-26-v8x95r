using BlobSimulation.Core.Logger;

namespace BlobSimulation.ConsoleRunner.ConsoleCommander.Commands {

    [CommandName("save")]
    public sealed class SaveCommand : ConsoleCommand {
        public string Path { get; }
        public SaveCommand() : this(string.Empty) { }

        public SaveCommand(string path) {
            Path = path;
        }

        public override CommandParseResult Parse(string[] parts) {
            string rawInput = string.Join(" ", parts);
            if (parts.Length < 2)
                return CommandParseResult.Error(rawInput, "Save command requires a path.");

            return CommandParseResult.Ok(new SaveCommand(parts[1]), rawInput);
        }

        public override bool Execute(CommandContext context) {
            context.Simulation.SaveSimulation(Path);
            context.Logger.Log($"Saved simulation to {Path}.", LogLevel.Success, "Save");
            return false;
        }
    }
}
