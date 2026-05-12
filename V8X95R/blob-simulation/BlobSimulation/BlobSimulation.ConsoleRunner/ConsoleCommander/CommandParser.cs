using BlobSimulation.ConsoleRunner.ConsoleCommander.Commands;
using BlobSimulation.ConsoleRunner.Renderer;
using BlobSimulation.Core;
using BlobSimulation.Core.Logger;
using System;
using System.Collections.Generic;

namespace BlobSimulation.ConsoleRunner.ConsoleCommander {
    public sealed class CommandContext {
        public SimulationEngine Simulation { get; }
        public ConsoleCamera Camera { get; }
        public ILogger Logger { get; }
        public IReadOnlyList<string> AvailableCommands { get; }


        public CommandContext(
            SimulationEngine simulation,
            ConsoleCamera camera,
            ILogger logger,
            IEnumerable<string>? availableCommands = null
            ) {
            Simulation = simulation;
            Camera = camera;
            Logger = logger;
            AvailableCommands = (availableCommands ?? Enumerable.Empty<string>())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
    }

    public record CommandParseResult(bool Success, ConsoleCommand? Command, string? ErrorMessage, string RawInput) {
        public static CommandParseResult Ok(ConsoleCommand command, string rawInput) =>
            new CommandParseResult(true, command, null, rawInput);

        public static CommandParseResult Error(string rawInput, string errorMessage) =>
            new CommandParseResult(false, null, errorMessage, rawInput);
    }

    public class CommandParser {

        private readonly IReadOnlyDictionary<string, ConsoleCommand> _commands;

        public CommandParser(ICommandProvider commandProvider) {
            _commands = commandProvider.Commands;
        }

        public CommandParseResult Parse(string? input) {
            if (string.IsNullOrWhiteSpace(input))
                return CommandParseResult.Ok(new StepCommand(1), string.Empty);

            string[] parts = input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string commandKey = parts[0];

            if (_commands.TryGetValue(commandKey, out var prototype))
                return prototype.Parse(parts);

            string raw = input.Trim();
            return CommandParseResult.Ok(new UnknownCommand(raw), raw);
        }
    }
}
