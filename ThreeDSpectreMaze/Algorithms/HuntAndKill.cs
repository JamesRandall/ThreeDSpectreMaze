using System.Collections.Immutable;

namespace ThreeDSpectreMaze.Algorithms;

public static class HuntAndKill
{
    private enum LookingFor
    {
        Touched,
        Untouched
    };

    public static Direction[,] Algorithm(int width, int height,
        Action<Direction[,]>? observer = null)
    {
        observer ??= _ => { };
        return Algorithm(width, height, (map, _) => observer(map));
    }
    
    public static Direction[,] Algorithm(int width, int height, Action<Direction[,], ImmutableList<MapVector>>? observer = null)
    {
        observer ??= (_,_) => { };
        
        var map = new Direction[height, width];
        var random = new Random(Environment.TickCount);
        
        Direction[] GetPossibleDirections(MapVector position, LookingFor lookingFor)
        {
            bool IsValid(Direction value) =>
                lookingFor == LookingFor.Touched ? value != Direction.None : value == Direction.None;
            var cx = position.x;
            var cy = position.y;
            
            var north = cy > 0 && IsValid(map[cy - 1, cx]) ? new[] {Direction.N} : new Direction[] { };
            var east = cx < (width - 1) && IsValid(map[cy, cx + 1]) ? new[] {Direction.E} : new Direction[] { };
            var south = cy < (height - 1) && IsValid(map[cy + 1, cx]) ? new[] {Direction.S} : new Direction[] { };
            var west = cx > 0 && IsValid(map[cy, cx - 1]) ? new[] {Direction.W} : new Direction[] { };
            var possibleDirections = north.Concat(east).Concat(south).Concat(west).ToArray();
            return possibleDirections;
        }

        MapVector UpdateMap(MapVector mapVector, Direction direction)
        {
            var nextPosition = mapVector + Directions.Vector[direction];
            var oppositeDirection = Directions.Opposite[direction];
            map[mapVector.y, mapVector.x] |= direction;
            map[nextPosition.y, nextPosition.x] |= oppositeDirection;
            return nextPosition;
        }

        MapVector Hunt()
        {
            for (var mapY = 0; mapY < height; mapY++)
            {
                for (var mapX = 0; mapX < width; mapX++)
                {
                    if (map[mapY, mapX] != Direction.None) continue;
                    
                    var position = new MapVector(mapX, mapY);
                    var possibleDirections = GetPossibleDirections(position, LookingFor.Touched);
                    if (possibleDirections.Any())
                    {
                        var direction = possibleDirections.MinBy(_ => random.Next());
                        UpdateMap(position, direction);
                        return position;
                    }
                    
                }
            }

            return MapVector.Invalid;
        }

        MapVector Walk(MapVector position)
        {
            var possibleDirections = GetPossibleDirections(position,LookingFor.Untouched);

            if (possibleDirections.Any())
            {
                var direction = possibleDirections.MinBy(_ => random.Next());
                return UpdateMap(position, direction);
            }

            return MapVector.Invalid;
        }

        var position = new MapVector(random.Next(width), random.Next(height));
        while (position.IsValid)
        {
            observer(map, ImmutableList<MapVector>.Empty);
            position = Walk(position);
            if (!position.IsValid) position = Hunt();
        }

        return map;
    }
}