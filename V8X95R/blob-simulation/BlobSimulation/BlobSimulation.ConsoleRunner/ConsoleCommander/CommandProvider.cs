using BlobSimulation.ConsoleRunner.ConsoleCommander.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BlobSimulation.ConsoleRunner.ConsoleCommander {

    public class CommandProvider : ICommandProvider {

        private readonly Dictionary<string, ConsoleCommand> _commands;

        public IReadOnlyDictionary<string, ConsoleCommand> Commands => _commands;

        public CommandProvider() {
            _commands = new Dictionary<string, ConsoleCommand>(StringComparer.OrdinalIgnoreCase);
            LoadNamedCommands();
        }

        private void LoadNamedCommands() {
            var candidates = typeof(ConsoleCommand).Assembly.GetTypes()
                .Where(t => !t.IsInterface && !t.IsAbstract)
                .Where(t => t.IsAssignableTo(typeof(ConsoleCommand)));

            foreach (var candidate in candidates) {
                var nameAttribute = candidate.GetCustomAttribute<CommandNameAttribute>();
                if (nameAttribute is null) continue;

                ConsoleCommand command = (ConsoleCommand)Activator.CreateInstance(candidate)!;
                _commands.Add(nameAttribute.Name, command);
            }
        }
    }
}

