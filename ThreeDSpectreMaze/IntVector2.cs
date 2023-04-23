namespace ThreeDSpectreMaze;

public record struct IntVector2(int x, int y)
{
    public static IntVector2 operator +(IntVector2 a, IntVector2 b) => new(a.x + b.x, a.y + b.y);
    public static IntVector2 operator *(IntVector2 a, int b) => new(a.x * b, a.y * b);
}