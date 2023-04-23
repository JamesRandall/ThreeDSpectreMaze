namespace ThreeDSpectreMaze.Algorithms;

public static class RecursiveBackTracking
{
    public static Direction[,] Algorithm(int width, int height, Action<Direction[,]>? observer = null)
    {
        observer ??= _ => { };
        
        var map = new Direction[height, width];
        var random = new Random(Environment.TickCount);
        
        void Walk(MapVector position)
        {
            foreach(var direction in Directions.All.OrderBy(_ => random.Next()).ToArray())
            {
                var next = position + Directions.Vector[direction];
                var oppositeDirection = Directions.Opposite[direction];
                if (next.WithinBounds(width,height) && map[next.y,next.x] == Direction.None)
                {
                    map[position.y,position.x] |= direction;
                    map[next.y, next.x] |= oppositeDirection;
                    observer(map);
                    Walk(next);
                }
            }
        }
        
        Walk(new MapVector(random.Next(width), random.Next(height)));

        return map;
    }
}