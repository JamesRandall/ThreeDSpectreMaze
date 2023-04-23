using System.Collections.Immutable;
using Spectre.Console;
using ThreeDSpectreMaze.Algorithms;

namespace ThreeDSpectreMaze;

public class Game
{
    private readonly ImmutableArray<ImmutableArray<Block>> _map;
    private readonly int _mapWidth;
    private readonly int _mapHeight;
    private enum MovementDirection { Forwards = 1, Backwards = -1}

    public Game(ImmutableArray<ImmutableArray<Block>> map)
    {
        _map = map;
        _mapWidth = map.First().Length;
        _mapHeight = map.Length;
        PlayerPosition = GetValidRandomPosition();
    }
    
    private MapVector PlayerPosition { get; set; }

    private Direction Facing { get; set; } = Direction.N;

    private bool IsValidPosition(MapVector position) => position.InBounds(_mapWidth, _mapHeight);

    private MapVector GetValidRandomPosition()
    {
        var random = new Random(Environment.TickCount);
        MapVector GetRandomPosition()
        {
            return new MapVector(
                random.Next(0, _mapWidth),
                random.Next(0, _mapHeight)
            );
        }
        
        var position = GetRandomPosition();
        while(_map[position.y][position.x] != Block.Empty)
        {
            position = GetRandomPosition();
        }

        return position;
    }

    private ImmutableArray<ImmutableArray<Block>> GetPlayerView()
    {
        var viewDirection = Directions.Vector[Facing];// new MapVector(Directions.X[Facing], Directions.Y[Facing]);
        // As we build up the view for the player we need to look at the cells to the left and right of them
        // and the vector for this is based on the direction they are facing
        (MapVector left, MapVector right) viewConeOffsets = Facing switch
        {
            Direction.N => (
                new MapVector(Directions.X[Direction.W], 0),
                new MapVector(Directions.X[Direction.E], 0)
            ),
            Direction.E => (
                new MapVector(0,Directions.Y[Direction.N]),
                new MapVector(0,Directions.Y[Direction.S])
            ),
            Direction.S => (
                new MapVector(Directions.X[Direction.E], 0),
                new MapVector(Directions.X[Direction.W], 0)
            ),
            Direction.W => (
                new MapVector(0,Directions.Y[Direction.S]),
                new MapVector(0,Directions.Y[Direction.N])
            ),
            _ => (MapVector.Zero, MapVector.Zero)
        };

        var view = new Block[Renderer.DrawDepth][];
        var position = PlayerPosition;
        for (var depth = 0; depth < Renderer.DrawDepth; depth++)
        {
            var leftPosition = position + viewConeOffsets.left;
            var rightPosition = position + viewConeOffsets.right;
            view[depth] = new[]
            {
                IsValidPosition(leftPosition) ? _map[leftPosition.y][leftPosition.x] : Block.Solid,
                IsValidPosition(position) ? _map[position.y][position.x] : Block.Solid,
                IsValidPosition(rightPosition) ? _map[rightPosition.y][rightPosition.x] : Block.Solid
            };
            position += viewDirection;
        }

        return view.Select(row => row.ToImmutableArray()).ToImmutableArray();
    }

    private void Move(MovementDirection direction)
    {
        var directionVector = new MapVector(Directions.X[Facing], Directions.Y[Facing]) * (int)direction;
        var newPosition = PlayerPosition + directionVector;
        if (IsValidPosition(newPosition) && _map[newPosition.y][newPosition.x] == Block.Empty)
        {
            PlayerPosition = newPosition;
        }
    }
    
    private void RotateLeft()
    {
        Facing = Facing switch
        {
            Direction.N => Direction.W,
            Direction.E => Direction.N,
            Direction.S => Direction.E,
            Direction.W => Direction.S,
            _ => Facing
        };
    }
    
    private void RotateRight()
    {
        Facing = Facing switch
        {
            Direction.N => Direction.E,
            Direction.E => Direction.S,
            Direction.S => Direction.W,
            Direction.W => Direction.N,
            _ => Facing
        };
    }

    public void Run(Canvas canvas)
    {
        var isOverhead = false;
        AnsiConsole.Live(canvas).Start(ctx =>
        {
            
            var quit = false;
            while (!quit)
            {
                if (isOverhead)
                {
                    Renderer.RenderOverhead(canvas, PlayerPosition, _map);
                }
                else
                {
                    Renderer.Render(canvas, GetPlayerView());
                }
                ctx.Refresh();
                var key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow: Move(MovementDirection.Forwards); break;
                    case ConsoleKey.DownArrow: Move(MovementDirection.Backwards); break;
                    case ConsoleKey.LeftArrow: RotateLeft(); break;
                    case ConsoleKey.RightArrow: RotateRight(); break;
                    case ConsoleKey.M: isOverhead = !isOverhead; break;
                    case ConsoleKey.Escape: quit = true; break;
                }
            }
        });
    }
}