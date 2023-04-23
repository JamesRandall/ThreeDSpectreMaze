namespace ThreeDSpectreMaze;

public readonly record struct MapVector(int x, int y)
{
    public static MapVector operator +(MapVector a, MapVector b) => new(a.x + b.x, a.y + b.y);
    public static MapVector operator *(MapVector a, int b) => new(a.x * b, a.y * b);
    public bool IsValid => x >= 0 && y >= 0;
    public static readonly MapVector Invalid = new MapVector(-1, -1);
    public static readonly MapVector Zero = new MapVector(0, 0);
    public bool WithinBounds(int width, int height) => x >= 0 && y >= 0 && x < width && y < height;
}