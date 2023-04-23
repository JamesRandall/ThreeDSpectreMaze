using Spectre.Console;

namespace ThreeDSpectreMaze;

public static class TitleScreen
{
    public static void Show(Canvas canvas)
    {
        var colors = new[]
        {
            new Color(224, 224, 224),
            new Color(64, 64, 64)
        };
        var fontHeight = RetroFont.Height + 2;

        // Wrapping the canvas write in a live prevents the newline at the end of the render
        AnsiConsole.Live(canvas).Start(ctx =>
            {
                Renderer.DrawRectangle(canvas, 0, 0, canvas.Width, canvas.Height, Color.Grey);
                for (var colorIndex = colors.Length - 1; colorIndex >= 0; colorIndex--)
                {
                    var color = colors[colorIndex];
                    var centerX = canvas.Width / 2 + colorIndex;
                    var yOffset = colorIndex;
                    RetroFont.Render(canvas, color, new MapVector(centerX, 10 + yOffset), "3D",
                        RetroFont.Alignment.Center);
                    RetroFont.Render(canvas, color, new MapVector(centerX, 10 + fontHeight + yOffset), "Spectre",
                        RetroFont.Alignment.Center);
                    RetroFont.Render(canvas, color, new MapVector(centerX, 10 + fontHeight * 2 + yOffset), "Maze",
                        RetroFont.Alignment.Center);
                }

                ctx.Refresh();
                Console.ReadKey();
            }
        );
    }
}