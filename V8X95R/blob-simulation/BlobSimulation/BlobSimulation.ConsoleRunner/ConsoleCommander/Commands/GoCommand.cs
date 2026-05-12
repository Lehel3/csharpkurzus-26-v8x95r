using BlobSimulation.Core.Logger;

namespace BlobSimulation.ConsoleRunner.ConsoleCommander.Commands {
    [CommandName("go")]
    public sealed class GoCommand : ConsoleCommand {
        public override CommandParseResult Parse(string[] parts) {
            string rawInput = string.Join(" ", parts);

            if (parts.Length > 1)
                return CommandParseResult.Error(rawInput, "Usage: go");

            return CommandParseResult.Ok(new GoCommand(), rawInput);
        }

        public override bool Execute(CommandContext context) {
            context.Logger.Log("Entering continuous simulation mode...", LogLevel.Info, "CLI");
            return false;
        }
    }
}

