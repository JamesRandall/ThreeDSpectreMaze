using System.Collections.Immutable;
using Spectre.Console;

namespace ThreeDSpectreMaze;

public static class ConsoleObserver
{
    public static ImmutableArray<ImmutableArray<Block>> ObserveCreation(Canvas canvas,
        Func<int, int, Action<Direction[,]>?, Direction[,]> algorithm)
    {
        var map = ImmutableArray<ImmutableArray<Block>>.Empty;
        var quit = false;
        AnsiConsole.Live(canvas).Start(ctx =>
        {
            void Observe(Direction[,] mapInProgress)
            {
                if (!quit)
                {
                    var inProgressMap = MapFactory.TransformToGrid(mapInProgress);
                    Renderer.RenderOverhead(canvas, new MapVector(-1, -1), inProgressMap);
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