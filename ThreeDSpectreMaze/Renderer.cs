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

    public static void DrawRectangle(Canvas canvas, int x, int y, int width, int height, Color color)
    {
        for(var cx = x; cx < x + width; cx++)
        {
            for(var cy = y; cy < y + height; cy++)
            {
                canvas.SetPixel(cx,cy,color);
            }
        }
    }
    
    private static readonly ImmutableArray<(int depth, Color color)> Walls = new []
    {
        (2, Color.Black), // walls alongside the player - they can just see the edge of them
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
        MapVector playerPosition,
        ImmutableArray<ImmutableArray<Block>> map)
    {
        DrawRectangle(canvas, 0,0,canvas.Width,canvas.Height,Color.Black);
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
    
    private const int PlayerLeft = 0;
    private const int PlayerMiddle = 1;
    private const int PlayerRight = 2;
    
    public static void Render(Canvas canvas, ImmutableArray<ImmutableArray<Block>> playerView)
    {
        DrawRectangle(canvas, 0,0,canvas.Width,canvas.Height,BackgroundColor);
        for (var depth = 0; depth < playerView.Length; depth++)
        {
            var row = playerView[depth];
            var startOffset = Walls.Take(depth).Sum(f => f.depth);
            var endOffset = startOffset + Walls[depth].depth - 1;
            var color = Walls[depth].color;
            
            DrawSidewalls(canvas, startOffset, endOffset, row, color);
            DrawFacingWalls(canvas, row, startOffset, endOffset);

            // Stop drawing if their is a block in front of us
            if (row[PlayerMiddle] == Block.Solid) break;
        }
    }

    private static void DrawFacingWalls(Canvas canvas, ImmutableArray<Block> row, int startOffset,
        int endOffset)
    {
        if (row[PlayerMiddle] == Block.Solid)
        {
            // Draw facing wall
            DrawRectangle(
                canvas,
                startOffset,
                startOffset,
                (canvas.Width - startOffset * 2),
                (canvas.Height - startOffset * 2),
                HorizontalWallColor
            );
        }

        if (row[PlayerLeft] == Block.Empty)
        {
            // Drawing facing wall of corridor to the left
            DrawRectangle(
                canvas,
                startOffset,
                (endOffset + 1),
                (endOffset - startOffset + 1),
                (canvas.Height - endOffset * 2 - 2),
                HorizontalWallColor
            );
        }

        if (row[PlayerRight] == Block.Empty)
        {
            // Draw facing wall of corridor to the right
            var wallWidth = (endOffset - startOffset);
            DrawRectangle(
                canvas,
                (canvas.Width - startOffset - wallWidth - 1),
                (endOffset + 1),
                wallWidth + 1,
                (canvas.Height - endOffset * 2 - 2),
                HorizontalWallColor
            );
        }
    }

    private static void DrawSidewalls(Canvas canvas, int startOffset, int endOffset, ImmutableArray<Block> row, 
        Color color)
    {
        for (var drawOffset = startOffset; drawOffset <= endOffset; drawOffset++)
        {
            if (row[PlayerLeft] == Block.Solid) DrawVerticalLine(canvas, drawOffset, color);
            if (row[PlayerRight] == Block.Solid) DrawVerticalLine(canvas, canvas.Width - drawOffset - 1, color);
        }
    }
}