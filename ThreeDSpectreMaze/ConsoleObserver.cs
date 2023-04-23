using System.Collections.Immutable;
using Spectre.Console;

namespace ThreeDSpectreMaze;

public static class ConsoleObserver
{
    public static ImmutableArray<ImmutableArray<Block>> ObserveCreation(
        Func<int, int, Action<int[,]>?, int[,]> algorithm)
    {
        var map = ImmutableArray<ImmutableArray<Block>>.Empty;
        var canvas = new Canvas(Renderer.CanvasWidth, Renderer.CanvasHeight);
        var quit = false;
        AnsiConsole.Live(canvas).Start(ctx =>
        {
            void Observe(int[,] mapInProgress)
            {
                if (!quit)
                {
                    var inProgressMap = MapFactory.TransformToGrid(mapInProgress);
                    Renderer.RenderOverhead(canvas, new IntVector2(-1, -1), inProgressMap);
                    ctx.Refresh();
                    Thread.Sleep(50);
                    while (Console.KeyAvailable)
                    {
                        quit |= Console.ReadKey().Key == ConsoleKey.Escape;
                    }
                }
            }

            map = MapFactory.Create(algorithm, Observe);
        });
        
        return map;
    }
    
    
}