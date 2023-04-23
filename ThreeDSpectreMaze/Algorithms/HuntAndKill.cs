namespace ThreeDSpectreMaze.Algorithms;

public static class HuntAndKill
{
    private enum LookingFor
    {
        Touched,
        Untouched
    };
    
    public static int[,] Algorithm(int width, int height, Action<int[,]>? observer = null)
    {
        observer ??= (_ => { });
        
        var map = new int[height, width];
        var random = new Random(Environment.TickCount);
        
        Direction[] GetPossibleDirections(int cx, int cy, LookingFor lookingFor)
        {
            bool IsValid(int value) => lookingFor == LookingFor.Touched ? value != 0 : value == 0; 
            
            var north = cy > 0 && IsValid(map[cy - 1, cx]) ? new[] {Direction.N} : new Direction[] { };
            var east = cx < (width - 1) && IsValid(map[cy, cx + 1]) ? new[] {Direction.E} : new Direction[] { };
            var south = cy < (height - 1) && IsValid(map[cy + 1, cx]) ? new[] {Direction.S} : new Direction[] { };
            var west = cx > 0 && IsValid(map[cy, cx - 1]) ? new[] {Direction.W} : new Direction[] { };
            var possibleDirections = north.Concat(east).Concat(south).Concat(west).ToArray();
            return possibleDirections;
        }

        (int x, int y) Hunt()
        {
            for (var mapY = 0; mapY < height; mapY++)
            {
                for (var mapX = 0; mapX < width; mapX++)
                {
                    if (map[mapY, mapX] == 0)
                    {
                        var possibleDirections = GetPossibleDirections(mapX, mapY, LookingFor.Touched);
                        if (possibleDirections.Any())
                        {
                            var direction = possibleDirections.MinBy(_ => random.Next());
                            var nextX = mapX + Directions.X[direction];
                            var nextY = mapY + Directions.Y[direction];
                            var oppositeDirection = Directions.Opposite[direction];
                            map[mapY,mapX] |= (int)direction;
                            map[nextY, nextX] |= (int)oppositeDirection;
                            observer(map);
                            return (mapX, mapY);
                        }
                    }
                }
            }

            return (-1, -1);
        }

        (int x, int y) Walk(int cx, int cy)
        {
            var possibleDirections = GetPossibleDirections(cx,cy,LookingFor.Untouched);

            if (possibleDirections.Any())
            {
                var direction = possibleDirections.MinBy(_ => random.Next());
                var nextX = cx + Directions.X[direction];
                var nextY = cy + Directions.Y[direction];
                var oppositeDirection = Directions.Opposite[direction];
                map[cy,cx] |= (int)direction;
                map[nextY, nextX] |= (int)oppositeDirection;
                observer(map);
                return (nextX, nextY);
            }

            return (-1, -1);
        }
        
        var (x, y) = (random.Next(width),random.Next(height));
        while (x >= 0 && y >= 0)
        {
            (x, y) = Walk(x,y);
            if (x == -1 && y == -1) (x, y) = Hunt();
        }

        return map;
    }
}