using System.Diagnostics;

public static class Test
{
    public static void Main()
    {
        var curentProcess = Process.GetCurrentProcess();
        var curentThread = Thread.CurrentThread;
        curentProcess.PriorityClass = ProcessPriorityClass.High;
        curentThread.Name = "MAIN";
        curentThread.Priority = ThreadPriority.Highest;

        var game = new Game();

        var watch = Stopwatch.StartNew();
        for (int i = 0; i < 3000; i++) Game.Update(game, 20);
        watch.Stop();

        System.Console.WriteLine($"Boids_C# {game.boids[0].ToString()} {watch.ElapsedMilliseconds}");
    }
}