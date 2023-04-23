using System.Collections.Immutable;
using Spectre.Console;

namespace ThreeDSpectreMaze;

public static class Renderer
{
    public const int CanvasWidth = 50;
    public const int CanvasHeight = 48;
    
    private static void DrawVerticalLine(Canvas canvas, int column, Color color)
    {
        (column < canvas.Width / 2
            ? Enumerable.Range(column, canvas.Height - column*2)
            : Enumerable.Range(canvas.Width - column - 1, canvas.Height - (canvas.Width - column - 1)*2)
        ).ForEach(y => canvas.SetPixel(column, y, color));
    }

    private static void DrawRectangle(Canvas canvas, int x, int y, int width, int height, Color color)
    {
        for(var cx = x; cx <= x + width; cx++)
        {
            for(var cy = y; cy <= y + height; cy++)
            {
                canvas.SetPixel(cx,cy,color);
            }
        }
    }
    
    private static readonly ImmutableArray<(int offset, Color color)> Walls = new []
    {
        (2, Color.Black),
        (8, Color.Black),
        (6, new Color(32, 32, 32)),
        (4, new Color(48, 48, 48)),
        (2, new Color(64, 64, 64)),
        (1, new Color(80, 80, 80))
    }.ToImmutableArray();

    public static int DrawDepth => Walls.Length;
    private static readonly Color BackgroundColor = new Color(224, 224, 224);
    private static readonly Color HorizontalWallColor = new Color(128, 128, 128);

    public static void RenderOverhead(
        Canvas canvas,
        IntVector2 playerPosition,
        ImmutableArray<ImmutableArray<Block>> map)
    {
        DrawRectangle(canvas, 0,0,canvas.Width-1,canvas.Height-1,Color.Black);
        for (var y = 0; y < map.Length; y++)
        {
            var row = map[y];
            for (var x = 0; x < row.Length; x++)
            {
                if (playerPosition.x == x && playerPosition.y == y)
                {
                    canvas.SetPixel(x, y, Color.Red);
                }
                else
                {
                    canvas.SetPixel(x, y, row[x] == Block.Solid ? Color.Grey : Color.White);
                }
            }
        }
    }
    
    public static void Render(Canvas canvas, ImmutableArray<ImmutableArray<Block>> playerView)
    {
        const int playerLeft = 0;
        const int playerMiddle = 1;
        const int playerRight = 2;
        
        DrawRectangle(canvas, 0,0,canvas.Width-1,canvas.Height-1,BackgroundColor);
        for (var depth = 0; depth < playerView.Length; depth++)
        {
            var row = playerView[depth];
            var startOffset = Walls.Take(depth).Sum(f => f.offset);
            var endOffset = startOffset + Walls[depth].offset - 1;
            var color = Walls[depth].color;

            // Draw side walls
            for (var drawOffset = startOffset; drawOffset <= endOffset; drawOffset++)
            {
                if (row[playerLeft] == Block.Solid) DrawVerticalLine(canvas, drawOffset, color);
                if (row[playerRight] == Block.Solid) DrawVerticalLine(canvas, canvas.Width-drawOffset-1, color);
            }
            if (row[playerMiddle] == Block.Solid)
            {
                // Draw horizontal wall
                DrawRectangle(
                    canvas,
                    startOffset,
                    startOffset,
                    (canvas.Width - startOffset * 2 - 1),
                    (canvas.Height - startOffset * 2 - 1),
                    HorizontalWallColor
                );
            }
            if (row[playerLeft] == Block.Empty)
            {
                DrawRectangle(
                    canvas,
                    startOffset,
                    (endOffset+1),
                    (endOffset-startOffset),
                    (canvas.Height - endOffset * 2 - 3),
                    HorizontalWallColor
                );
            }
            if (row[playerRight] == Block.Empty)
            {
                var wallWidth = (endOffset - startOffset);
                DrawRectangle(
                    canvas,
                    (canvas.Width-startOffset-wallWidth-1),
                    (endOffset+1),
                    wallWidth,
                    (canvas.Height-endOffset*2-3),
                    HorizontalWallColor
                );
            }

            // Stop drawing if their is a block in front of us
            if (row[playerMiddle] == Block.Solid) break;
        }
    }
}