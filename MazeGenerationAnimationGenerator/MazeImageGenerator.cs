using System.Collections.Immutable;
using SixLabors.ImageSharp.Drawing.Processing;

namespace ThreeDSpectreMaze;

public static class MazeImageGenerator
{
    public static Image<Rgba32> GenerateImage(Direction[,] map, ImmutableList<MapVector> path, int frameNumber,
        int imageWidth, int imageHeight, string outputFolder)
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
                        ? path.Last().x == x && path.Last().y == y ? Color.LightBlue : Color.SkyBlue
                        : cell == Direction.None ? Color.DarkGrey : Color.White;
                
                image.Mutate(x =>
                    {
                        x.Fill(backgroundColor, new RectangleF(cellX, cellY, cellWidth+1, cellHeight+1));
                    }
                );
                
                
                if ((cell & Direction.N) == 0)
                {
                    image.Mutate(x => 
                        x.DrawLines(
                            Color.Black, 
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
                            Color.Black, 
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
                x.Draw(Color.Black, 1, new RectangleF(0, 0, imageWidth, imageHeight));
            }
        );
        return image;
    }
}