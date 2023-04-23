using System.Collections.Immutable;

namespace ThreeDSpectreMaze.Algorithms;

public static class Directions
{
    public static readonly ImmutableArray<Direction> All =
        new [] { Direction.N, Direction.E, Direction.S, Direction.W }.ToImmutableArray();

    public static readonly ImmutableDictionary<Direction, int> X =
        new Dictionary<Direction, int>
        {
            {Direction.N, 0},
            {Direction.E, 1},
            {Direction.S, 0},
            {Direction.W, -1}
        }.ToImmutableDictionary();
    
    public static readonly ImmutableDictionary<Direction, int> Y =
        new Dictionary<Direction, int>
        {
            {Direction.N, -1},
            {Direction.E, 0},
            {Direction.S, 1},
            {Direction.W, 0}
        }.ToImmutableDictionary();
    
    public static readonly ImmutableDictionary<Direction, Direction> Opposite =
        new Dictionary<Direction, Direction>
        {
            {Direction.N, Direction.S},
            {Direction.E, Direction.W},
            {Direction.S, Direction.N},
            {Direction.W, Direction.E}
        }.ToImmutableDictionary();
}