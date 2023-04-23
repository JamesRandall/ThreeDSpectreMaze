namespace ThreeDSpectreMaze.Algorithms;

public static class RecursiveBackTracking
{
    public static int[,] Algorithm(int width, int height)
    {
        var map = new int[height, width];
        var random = new Random(Environment.TickCount);
        
        void Walk(int cx, int cy)
        {
            foreach(var direction in Directions.All.OrderBy(_ => random.Next()).ToArray())
            {
                var nextX = cx + Directions.X[direction];
                var nextY = cy + Directions.Y[direction];
                var oppositeDirection = Directions.Opposite[direction];
                if (nextX >= 0 && nextY >= 0 && nextX < width && nextY < height && map[nextY,nextX] == 0)
                {
                    map[cy,cx] |= (int)direction;
                    map[nextY, nextX] |= (int)oppositeDirection;
                    Walk(nextX, nextY);
                }
            }
        }
        
        Walk(0,0);

        return map;
    }
}