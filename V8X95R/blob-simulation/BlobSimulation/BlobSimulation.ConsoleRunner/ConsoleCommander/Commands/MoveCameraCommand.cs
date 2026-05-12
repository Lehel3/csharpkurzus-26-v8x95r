using BlobSimulation.Core.Logger;

namespace BlobSimulation.ConsoleRunner.ConsoleCommander.Commands {

    [CommandName("cam")]
    public sealed class MoveCameraCommand : ConsoleCommand {
        public int Dx { get; }
        public int Dy { get; }
        public MoveCameraCommand() : this(0, 0) { }

        public MoveCameraCommand(int dx, int dy) {
            Dx = dx;
            Dy = dy;
        }


        public override CommandParseResult Parse(string[] parts) {
            string rawInput = string.Join(" ", parts);
            if (parts.Length < 2)
                return CommandParseResult.Error(rawInput, "Camera command requires a direction.");

            string direction = parts[1].ToLowerInvariant();
            int amount = 1;

            if (parts.Length >= 3 && !int.TryParse(parts[2], out amount))
                return CommandParseResult.Error(rawInput, "Camera amount must be a number.");

            return direction switch {
                "e" or "r" or "right" => CommandParseResult.Ok(new MoveCameraCommand(amount, 0), rawInput),
                "w" or "l" or "left" => CommandParseResult.Ok(new MoveCameraCommand(-amount, 0), rawInput),
                "n" or "u" or "up" => CommandParseResult.Ok(new MoveCameraCommand(0, amount), rawInput),
                "s" or "d" or "down" => CommandParseResult.Ok(new MoveCameraCommand(0, -amount), rawInput),
                _ => CommandParseResult.Error(rawInput, "Unknown camera direction.")
            };
        }

        public override bool Execute(CommandContext context) {
            context.Camera.CenterX += Dx;
            context.Camera.CenterY += Dy;
            context.Logger.Log($"Camera moved to ({context.Camera.CenterX}, {context.Camera.CenterY}).", LogLevel.Info, "Cam");
            return false;
        }
    }
}
