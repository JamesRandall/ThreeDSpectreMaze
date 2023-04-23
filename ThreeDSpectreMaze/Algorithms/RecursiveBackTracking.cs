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
                if (next.InBounds(width,height) && map[next.y,next.x] == 0)
                {
                    map[position.y,position.x] |= direction;
                    map[next.y, next.x] |= oppositeDirection;
                    observer(map);
                    Walk(next);
                }
            }
        }
        
        Walk(MapVector.Zero);

        return map;
    }
}