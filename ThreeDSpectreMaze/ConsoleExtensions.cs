namespace ThreeDSpectreMaze;

public static class ConsoleExtensions
{
    private static readonly ConsoleKeyInfo DefaultKey =
        new(' ', ConsoleKey.Spacebar, false, false, false);
    
    public static bool TryWaitReadKey(int durationMs, out ConsoleKeyInfo key, int intervalWaitMs=50)
    {
        int totalDuration = 0;
        while(!System.Console.KeyAvailable && totalDuration < durationMs)
        {
            Thread.Sleep(intervalWaitMs);
            totalDuration += intervalWaitMs;
        }

        if (System.Console.KeyAvailable)
        {
            key = System.Console.ReadKey();
            return true;
        }

        key = DefaultKey;
        return false;
    }
}