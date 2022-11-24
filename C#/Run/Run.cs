using System.Diagnostics;

public static class Run
{
    [STAThread]
    public static void Main()
    {
        var curentProcess = Process.GetCurrentProcess();
        var curentThread = Thread.CurrentThread;
        curentProcess.PriorityClass = ProcessPriorityClass.High;
        curentThread.Name = "MAIN";
        curentThread.Priority = ThreadPriority.Highest;
        var watch = new Stopwatch();

        var game = new Game();
        var windowThread = WindowProcess.Start(game);
        var timeStep = 20L;
        while (windowThread.IsAlive)
        {
            watch.Restart();
            Game.Update(game, timeStep);
            watch.Stop();
            var diff = watch.ElapsedMilliseconds;
            var waitTime = timeStep - diff;
            Wait(waitTime);
        }
    }
    public static void Wait(long milliseconds)
    {
        if (milliseconds <= 0) return;
        Thread.Sleep((int)milliseconds);
    }
}