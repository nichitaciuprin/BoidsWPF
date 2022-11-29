using System.Diagnostics;

public static class TestBoidWorld
{
    public static void Main()
    {
        var curentProcess = Process.GetCurrentProcess();
        var curentThread = Thread.CurrentThread;
        curentProcess.PriorityClass = ProcessPriorityClass.High;
        curentThread.Name = "MAIN";
        curentThread.Priority = ThreadPriority.Highest;

        var boidWorld = new BoidWorld();

        var watch = Stopwatch.StartNew();
        for (int i = 0; i < 3000; i++) boidWorld.Update(20);
        watch.Stop();

        System.Console.Write("Boids_C#");
        System.Console.Write(" ");
        boidWorld.boids[0].Print();
        System.Console.Write(" ");
        System.Console.Write(watch.ElapsedMilliseconds);
        System.Console.Write(" \n");
    }
}