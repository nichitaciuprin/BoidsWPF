using System.Diagnostics;

public unsafe static class Programm
{
    public unsafe static void Main()
    {
        Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
        Thread.CurrentThread.Priority = ThreadPriority.Highest;

        Game.Init();

        var watch = Stopwatch.StartNew();
        for (int i = 0; i < 2000; i++) Game.Update();
        watch.Stop();

        Console.WriteLine(watch.ElapsedMilliseconds);

        Game.End();
    }
}