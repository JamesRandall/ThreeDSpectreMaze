using System.Collections.Immutable;
using Spectre.Console;
using ThreeDSpectreMaze;
using ThreeDSpectreMaze.Algorithms;

var canvas = new Canvas(Renderer.CanvasWidth, Renderer.CanvasHeight);
TitleScreen.Show(canvas);
AnsiConsole.Clear();

var algorithms = new Dictionary<string, Func<int, int, Action<Direction[,]>?, Direction[,]>>
{
    {"Recursive back tracking", RecursiveBackTracking.Algorithm},
    {"Hunt and kill", HuntAndKill.Algorithm}
};

var algorithm =
    algorithms[
        AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Choose a maze generation algorithm")
            .PageSize(10)
            .AddChoices(algorithms.Keys))
    ];
var observe =
    AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Observe maze generation")
            .PageSize(10)
            .AddChoices(new[] {"Yes", "No"}));

AnsiConsole.Clear();

var map =
    observe == "Yes"
        ? ConsoleObserver.ObserveCreation(canvas, algorithm)
        : MapFactory.Create(algorithm);

var game = new Game(map);
game.Run(canvas);
