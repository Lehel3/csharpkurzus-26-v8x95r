namespace BlobSimulation.ConsoleRunner.ConsoleCommander.Commands {

    public abstract class ConsoleCommand {

        public abstract CommandParseResult Parse(string[] parts);
        public abstract bool Execute(CommandContext context);

    }

}
