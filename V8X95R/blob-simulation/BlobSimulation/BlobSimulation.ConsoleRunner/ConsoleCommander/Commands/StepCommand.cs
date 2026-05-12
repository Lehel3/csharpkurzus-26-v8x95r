using BlobSimulation.Core.Logger;

namespace BlobSimulation.ConsoleRunner.ConsoleCommander.Commands {

    [CommandName("step")]
    public sealed class StepCommand : ConsoleCommand {
        public int Count { get; }
        public StepCommand() : this(1) { }

        public StepCommand(int count) {
            Count = count;
        }

        public override CommandParseResult Parse(string[] parts) {
            string rawInput = string.Join(" ", parts);

            if (parts.Length == 1)
                return CommandParseResult.Ok(new StepCommand(1), rawInput);

            if (!int.TryParse(parts[1], out int count))
                return CommandParseResult.Error(rawInput, "Step count must be a number.");

            if (count < 1)
                return CommandParseResult.Error(rawInput, "Invalid step count.");

            return CommandParseResult.Ok(new StepCommand(count), rawInput);
        }

        public override bool Execute(CommandContext context) {
            bool cappedStepMode = context.Simulation.Config.MaxSteps > 0;
            int stepCount = cappedStepMode
                ? Math.Min(Count, context.Simulation.Config.MaxSteps - context.Simulation.Tick)
                : Count;

            for (int i = 0; i < stepCount - 1; i++)
                context.Simulation.Step();

            context.Logger.Log($"Stepped simulation {stepCount} time(s).", LogLevel.Success, "CLI");
            return true;
        }
    }
}
