namespace ThreeDSpectreMaze;

public static class LinqExtensions
{
    public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
    {
        foreach(T item in enumeration)
        {
            action(item);
        }
    }
    
    public static void ForEachI<T>(this IEnumerable<T> enumeration, Action<int,T> action)
    {
        var index = 0;
        foreach(T item in enumeration)
        {
            action(index, item);
            index++;
        }
    }
}