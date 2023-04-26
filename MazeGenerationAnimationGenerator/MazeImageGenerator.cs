using System.Collections.Immutable;
using SixLabors.ImageSharp.Drawing.Processing;

namespace ThreeDSpectreMaze;

public static class MazeImageGenerator
{
    private static readonly Color VisitedCellColor = Color.White;
    private static readonly Color PathTipColor = Color.LightBlue;
    private static readonly Color GridLineColor = Color.Black;
    private static readonly Color UnvisitedCellColor = Color.DarkGrey;
    private static readonly Color PathColor = Color.SkyBlue;
    
    public static ImmutableArray<Color> Palette = new []
    {
        PathTipColor,
        PathColor,
        VisitedCellColor,
        GridLineColor,
        UnvisitedCellColor,
        // The below color is needed due to the compression of the gif, without it you get some unfortunate artifacts
        // from the other colors, with it the artifact lands in an unnoticeable place.
        Color.FromRgb(47, 79, 79)
    }.ToImmutableArray();

    public static Image<Rgba32> GenerateImage(
        Direction[,] map,
        ImmutableList<MapVector> path,
        int imageWidth,
        int imageHeight)
    {
        var mazeHeight = map.GetLength(0);
        var mazeWidth = map.GetLength(1);
        var image = new Image<Rgba32>(imageWidth, imageHeight);
        var cellWidth = imageWidth / (float)mazeWidth;
        var cellHeight = imageHeight / (float)mazeHeight;
        
        for (var y = 0; y < mazeHeight; y++)
        {
            for (var x = 0; x < mazeWidth; x++)
            {
                var cell = map[y, x];
                var cellX = x * cellWidth;
                var cellY = y * cellHeight;
                
                var backgroundColor =
                    path.Any(mv => mv.x == x && mv.y == y)
                        ? path.Last().x == x && path.Last().y == y ? PathTipColor : PathColor
                        : cell == Direction.None ? UnvisitedCellColor : VisitedCellColor;
                
                image.Mutate(x =>
                    {
                        x.Fill(backgroundColor, new RectangleF(cellX, cellY, cellWidth+1, cellHeight+1));
                    }
                );
                
                
                if ((cell & Direction.N) == 0)
                {
                    image.Mutate(x => 
                        x.DrawLines(
                            GridLineColor, 
                            1,
                            new PointF(cellX, cellY),
                            new PointF(cellX + cellWidth, cellY)
                        )
                    );
                }
                if ((cell & Direction.W) == 0)
                {
                    image.Mutate(x =>
                        x.DrawLines(
                            GridLineColor, 
                            1,
                            new PointF(cellX, cellY),
                            new PointF(cellX, cellY + cellHeight)
                        )
                    );
                }
            }
        }
        image.Mutate(x =>
            {
                x.Draw(GridLineColor, 1, new RectangleF(0, 0, imageWidth, imageHeight));
            }
        );
        return image;
    }
}