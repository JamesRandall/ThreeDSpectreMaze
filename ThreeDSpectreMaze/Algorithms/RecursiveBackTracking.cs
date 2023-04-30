using System.Collections.Immutable;

namespace ThreeDSpectreMaze.Algorithms;

public static class RecursiveBackTracking
{
    public static Direction[,] Algorithm(int width, int height, Action<Direction[,]>? observer = null)
    {
        observer ??= _ => { };
        return Algorithm(false, width, height, (map,path) => observer(map));
    }
    
    public static Direction[,] Algorithm(int width, int height, Action<Direction[,],ImmutableList<MapVector>>? observer = null)
    {
        return Algorithm(true, width, height, observer);
    }

    private static Direction[,] Algorithm(
        bool trackPath,
        int width,
        int height,
        Action<Direction[,], ImmutableList<MapVector>>? observer = null)
    {
        observer ??= (_,_) => { };
        
        var map = new Direction[height, width];
        var random = new Random(Environment.TickCount);
        
        void Walk(MapVector position, ImmutableList<MapVector> path)
        {
            path = path.Add(position);
            observer(map, path);
            foreach(var direction in Directions.All.OrderBy(_ => random.Next()).ToArray())
            {
                var next = position + Directions.Vector[direction];
                var oppositeDirection = Directions.Opposite[direction];
                if (next.WithinBounds(width,height) && map[next.y,next.x] == Direction.None)
                {
                    map[position.y,position.x] |= direction;
                    map[next.y, next.x] |= oppositeDirection;
                    Walk(next, path);
                    if (trackPath) observer(map, path);
                }
            }
        }
        
        // only do this if we're observing back tracks
        if (trackPath) observer(map, ImmutableList<MapVector>.Empty);
        
        var start = new MapVector(random.Next(width), random.Next(height));
        Walk(start, new [] { start}.ToImmutableList());
        
        // only do this if we're observing back tracks
        if (trackPath) observer(map, ImmutableList<MapVector>.Empty);

        return map;
    }
}