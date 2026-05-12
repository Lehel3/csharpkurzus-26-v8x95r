using BlobSimulation.Core.Logger;

namespace BlobSimulation.ConsoleRunner.ConsoleCommander.Commands {

    [CommandName("help")]
    public sealed class HelpCommand : ConsoleCommand {

        public override CommandParseResult Parse(string[] parts) {
            return CommandParseResult.Ok(new HelpCommand(), string.Join(" ", parts));
        }

        public override bool Execute(CommandContext context) {
            if (context.AvailableCommands.Count == 0) {
                context.Logger.Log("No commands are currently registered.", LogLevel.Warning, "CLI");
                return false;
            }

            string commandList = string.Join(", ", context.AvailableCommands);
            context.Logger.Log($"Commands: {commandList}", LogLevel.Info, "CLI");
            return false;
        }
    }
}
