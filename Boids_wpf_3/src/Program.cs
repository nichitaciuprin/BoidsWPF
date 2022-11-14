using System.Diagnostics;

public unsafe static class Programm
{
    public unsafe static void Main()
    {
        Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
        Thread.CurrentThread.Priority = ThreadPriority.Highest;

        Game.Init();
        var watch = Stopwatch.StartNew();
        for (int i = 0; i < 3000; i++) Game.Update();
        watch.Stop();
        Console.WriteLine($"ms: {watch.ElapsedMilliseconds}");
        Game.End();

        // GameEngine.MainInit();
        // var watch = Stopwatch.StartNew();
        // for (int i = 0; i < 1000; i++) GameEngine.MainUpdate(Helper.ToTicks(20));
        // watch.Stop();
        // Console.WriteLine($"ms: {watch.ElapsedMilliseconds}");
        // GameEngine.MainEnd();
    }
}