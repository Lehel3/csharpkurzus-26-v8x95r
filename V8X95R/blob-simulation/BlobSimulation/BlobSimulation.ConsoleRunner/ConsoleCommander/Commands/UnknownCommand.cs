using BlobSimulation.Core.Logger;

namespace BlobSimulation.ConsoleRunner.ConsoleCommander.Commands {

    public sealed class UnknownCommand : ConsoleCommand {
        public string Input { get; }
        public UnknownCommand(string input) {
            Input = input;
        }


        public override CommandParseResult Parse(string[] parts) {
            string rawInput = string.Join(" ", parts);
            return CommandParseResult.Ok(new UnknownCommand(rawInput), rawInput);
        }

        public override bool Execute(CommandContext context) {
            context.Logger.Log($"Unknown command: '{Input}'. Type 'help' for a list of commands.", LogLevel.Warning, "CLI");
            return false;
        }
    }
}
