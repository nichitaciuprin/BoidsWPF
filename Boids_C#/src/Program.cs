using System.Diagnostics;
using System.Threading;

public static class Program
{
    public static void Main()
    {
        var curentProcess = Process.GetCurrentProcess();
        var curentThread = Thread.CurrentThread;
        curentProcess.PriorityClass = ProcessPriorityClass.High;
        curentThread.Name = "MAIN";
        curentThread.Priority = ThreadPriority.Highest;

        var gameState = Game.Init();

        var watch = Stopwatch.StartNew();
        for (int i = 0; i < 3000; i++) Game.Update(gameState, 20);
        watch.Stop();

        System.Console.WriteLine($"Boids_C# {gameState.boids[0].ToString()} {watch.ElapsedMilliseconds}");

        Game.End(gameState);
    }
}