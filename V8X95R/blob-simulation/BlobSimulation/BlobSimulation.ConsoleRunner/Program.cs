using BlobSimulation.ConsoleRunner;
using BlobSimulation.ConsoleRunner.ConsoleCommander;
using BlobSimulation.ConsoleRunner.ConsoleCommander.Commands;
using BlobSimulation.ConsoleRunner.Renderer;
using BlobSimulation.ConsoleRunner.Renderer.Views;
using BlobSimulation.Core;
using BlobSimulation.Core.Logger;
using BlobSimulation.Core.SimObjects;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading;
using static BlobSimulation.Core.SimulationEngine;

public class Program
{

    public static void Main(string[] args)
    {
        try
        {
            int consoleWidth = 140;
            int consoleHeight = 40;

            Console.SetWindowSize(consoleWidth, consoleHeight);
            Console.SetBufferSize(consoleWidth, consoleHeight);
            Console.WriteLine($"Console size: {Console.WindowWidth}x{Console.WindowHeight}\n" +
                $"WARNING: Auto resize doesn't work on some consoles and environments.\n" +
                $"Please resize console to a reasonable size before continouing.");

            using var fileSink = new FileLogSink("./logs/simulation.log");
            var consoleSink = new ConsoleLogSink();
            var logger = new ConsoleLogger(fileSink, consoleSink);

            Config config = ConsoleConfigLoader.Load(logger);
            var sim = new SimulationEngine(config, logger, true);


            int tickRate = config.TickRate;

            var camera = new ConsoleCamera(
                centerX: 10,
                centerY: 8,
                width: 32,
                height: 24
            );

            var layout = CreateLayout(camera, out var logHistoryView, out var worldView);
            Console.WriteLine("Press any key to start the simulation...");
            Console.ReadKey();
            Console.Clear();

            logger.Mode = LogMode.Buffered;

            var commandProvider = new CommandProvider();
            var commandParser = new CommandParser(commandProvider);

            bool canStepForward = false;
            bool extinct = false;
            // loop
            bool cappedStepMode = config.MaxSteps > 0;
            bool canStep = cappedStepMode ? sim.Tick < config.MaxSteps : true;
            while (canStep)
            {
                sim.Step();

                extinct = CheckEarlyStopCondition(sim);
                if (extinct)
                {
                    logger.Log($"Simulation stopped early, all the blobs died at Tick {sim.Tick}. :(");
                    logHistoryView.SetLogs(logger.GetBufferedLogs());
                    layout.Draw(new ConsoleRenderContext(sim, camera));
                    Console.ReadKey();
                    break;
                }

                canStepForward = false;
                while (!canStepForward)
                {
                    logHistoryView.SetLogs(logger.GetBufferedLogs());
                    layout.Draw(new ConsoleRenderContext(sim, camera));

                    
                    string? input = Console.ReadLine();

                    var parseResult = commandParser.Parse(input);
                    logger.Log($"> {input}", LogLevel.Info, "CLI");
                    if (!parseResult.Success) {
                        logger.Log($"Command error: {parseResult.ErrorMessage} in \"{parseResult.RawInput}\"", LogLevel.Error, "CLI");
                        continue;
                    }

                    if (parseResult.Command is GoCommand) {
                        bool shouldStop = RunContinuousMode(sim, camera, worldView, logger, tickRate);
                        if (shouldStop) {
                            canStep = false;
                            canStepForward = true;
                            break;
                        }

                        Console.Clear();
                        continue;
                    }

                    try {
                        canStepForward = parseResult.Command!.Execute(
                            new CommandContext(sim, camera, logger, commandProvider.Commands.Keys));
                    }
                    catch (Exception ex) {
                        logger.Log($"Command execution failed: {ex.Message}", LogLevel.Error, "CLI");
                    }
                }

                logger.ClearBuffer();
                canStep = cappedStepMode ? sim.Tick < config.MaxSteps : true;
            }
        }
        catch (FileNotFoundException ex)
        {
            WriteError("[Error] Configuration file not found.", ex);
        }
        catch (JsonException ex)
        {
            WriteError("[Error] Invalid JSON in configuration file.", ex);
        }
        catch (ArgumentException ex)
        {
            WriteError("[Error] Invalid configuration values.", ex);
        }
        catch (Exception ex)
        {
            WriteError("[Error] Unexpected application error.", ex);
        }
    }

    public static void WriteError(string message, Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.WriteLine(ex.Message);
        Console.ResetColor();   
    }


    public static bool CheckEarlyStopCondition(SimulationEngine sim) {
        bool extinct = sim.blobCount == 0;
        return extinct;
    }

    public static ConsoleLayout CreateLayout(ConsoleCamera camera, out LogHistoryView logHistory, out WorldView worldView) {
        worldView = new WorldView(camera) {
            X = 2, Y = 1
        };

        var promptView = new PromptView() {
            X = 2, Y = worldView.EndY + 2,
        };

        logHistory = new LogHistoryView() {
            X = worldView.EndX + 2, Y = 1,
            Height = 24
        };

        var layout = new ConsoleLayout();
        layout.Add(worldView);
        layout.Add(logHistory);
        layout.Add(promptView);
        
        return layout;
    }

    private static bool RunContinuousMode(SimulationEngine sim, ConsoleCamera camera, WorldView worldView, ConsoleLogger logger, int tickRate) {
        bool consoleAvailable = true;
        try {
            Console.Clear();
        }
        catch (IOException) {
            consoleAvailable = false;
        }
        catch (InvalidOperationException) {
            consoleAvailable = false;
        }

        logger.Log("Continuous simulation started. Press ESC to exit.", LogLevel.Info, "CLI");

        double stepIntervalMs = Math.Max(1, 1000.0 / Math.Max(1, tickRate));
        int sleepMs = (int)Math.Max(1, Math.Round(stepIntervalMs));

        var timer = Stopwatch.StartNew();
        double accumulatedMs = 0;

        while (true) {
            if (consoleAvailable) {
                bool hasKey;
                try {
                    hasKey = Console.KeyAvailable;
                }
                catch (IOException) {
                    hasKey = false;
                    consoleAvailable = false;
                }
                catch (InvalidOperationException) {
                    hasKey = false;
                    consoleAvailable = false;
                }

                while (hasKey) {
                    var key = Console.ReadKey(intercept: true);
                    if (key.Key == ConsoleKey.Escape) {
                        logger.Log("Continuous simulation stopped.", LogLevel.Info, "CLI");
                        return false;
                    }

                    try {
                        hasKey = Console.KeyAvailable;
                    }
                    catch (IOException) {
                        hasKey = false;
                        consoleAvailable = false;
                    }
                    catch (InvalidOperationException) {
                        hasKey = false;
                        consoleAvailable = false;
                    }
                }
            }

            double frameMs = timer.Elapsed.TotalMilliseconds;
            timer.Restart();
            accumulatedMs += frameMs;

            while (accumulatedMs >= stepIntervalMs) {
                bool cappedStepMode = sim.Config.MaxSteps > 0;
                if (cappedStepMode && sim.Tick >= sim.Config.MaxSteps) {
                    logger.Log("Reached max step count in continuous mode.", LogLevel.Warning, "CLI");
                    return false;
                }

                sim.Step();
                accumulatedMs -= stepIntervalMs;

                if (CheckEarlyStopCondition(sim)) {
                    logger.Log($"Simulation stopped early, all blobs died at Tick {sim.Tick}.", LogLevel.Warning, "CLI");
                    return true;
                }
            }

            if (consoleAvailable) {
                worldView.Draw(new ConsoleRenderContext(sim, camera));
                int infoX = worldView.EndX + 3;
                ConsoleDrawUtils.WriteAt(infoX, 1, $"Tick: {sim.Tick}", 40);
                ConsoleDrawUtils.WriteAt(infoX, 2, "Continuous mode", 40);
                ConsoleDrawUtils.WriteAt(infoX, 3, "Press ESC to return", 40);
            }

            Thread.Sleep(sleepMs);
        }
    }

    private static void DumpAllBlobs(SimulationEngine sim)
    {
        foreach (var blob in sim.World.SimObjects.OfType<Blob>())
        {
            sim.Logger.Log($"[{blob.ToString()}]: {blob.state} e: {blob.Energy}, pos: ({blob.Position.X.ToString("#.#")}, {blob.Position.Y.ToString("#.#")})");
        }
    }
}
