using System.Diagnostics;

public static class Program
{
    public static void Main()
    {
        Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
        Thread.CurrentThread.Priority = ThreadPriority.Highest;

        var gameState = Game.Init();

        var watch = Stopwatch.StartNew();
        for (int i = 0; i < 3000; i++) Game.Update(ref gameState, 20);
        watch.Stop();

        Game.End(ref gameState);

        Console.WriteLine($"ms: {watch.ElapsedMilliseconds}");
    }
}