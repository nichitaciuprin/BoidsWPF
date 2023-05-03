using System.Diagnostics;

public static class Game
{
    public static void Main()
    {
        var curentProcess = Process.GetCurrentProcess();
        var curentThread = Thread.CurrentThread;
        curentProcess.PriorityClass = ProcessPriorityClass.High;
        curentThread.Name = "MAIN";
        curentThread.Priority = ThreadPriority.Highest;
        var watch = new Stopwatch();

        var gameWorld = new GameWorld();
        var gameWorldWindow = GameWorldWindow.Start(gameWorld);
        var timeStep = 20L;
        while (gameWorldWindow.IsAlive)
        {
            watch.Restart();
            gameWorld.Update(timeStep);
            watch.Stop();
            var diff = watch.ElapsedMilliseconds;
            var waitTime = timeStep - diff;
            Wait(waitTime);
        }
    }
    public static void Wait(long milliseconds)
    {
        if (milliseconds <= 0)
        {
            Console.WriteLine("SLOW");
            return;
        }
        Thread.Sleep((int)milliseconds);
    }
}