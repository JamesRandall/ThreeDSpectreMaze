using System.Collections.Immutable;
using ThreeDSpectreMaze.Algorithms;

namespace ThreeDSpectreMaze;

public enum Block { Solid, Empty }

public static class MapFactory
{
    const int Width = 17;
    const int Height = 17;
    
    public static ImmutableArray<ImmutableArray<Block>> Create(Func<int, int, int[,]> algorithm)
    {
        var directionalMap = algorithm(Width, Height);
        var gridMap = TransformToGrid(directionalMap);
        return gridMap;
    }
    
    private static ImmutableArray<ImmutableArray<Block>> TransformToGrid(int[,] map)
    {
        var topRow = Enumerable.Repeat(Block.Solid, Width * 2 + 1).ToList();
        var rows = new List<List<Block>>();
        rows.Add(topRow);
        for (var y = 0; y < Height; y++)
        {
            var row1 = new List<Block>();
            var row2 = new List<Block>();
            // our rows always start with a wall otherwise the maze would be open
            // because we are checking eastwards we don't need to terminate with a
            // wall, the maze generation algorithm itself terminates and all last columns
            // will have no eastward openings
            row1.Add(Block.Solid);
            row2.Add(Block.Solid);
            for (var x = 0; x < Width; x++)
            {
                row1.Add(Block.Empty);
                row1.Add((map[y, x] & (int)Direction.E) > 0 ? Block.Empty : Block.Solid);
                
                row2.Add((map[y, x] & (int)Direction.S) > 0 ? Block.Empty : Block.Solid);
                row2.Add(Block.Solid);
            }
            rows.Add(row1);
            rows.Add(row2);
        }

        var result = rows.Select(row => row.ToImmutableArray()).ToImmutableArray();
        return result;
    }
}