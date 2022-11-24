using System.Diagnostics;

public static class App
{
    [STAThread]
    public static void Main()
    {
        var curentProcess = Process.GetCurrentProcess();
        var curentThread = Thread.CurrentThread;
        curentProcess.PriorityClass = ProcessPriorityClass.High;
        curentThread.Name = "MAIN";
        curentThread.Priority = ThreadPriority.Highest;

        var game = new Game();
        var watch = new Stopwatch();
        var timeStep = 20;
        var windowThread = StartWindowThread(game);
        while (windowThread.IsAlive)
        {
            watch.Restart();
            Game.Update(game, timeStep);
            watch.Stop();
            var waitTime = timeStep - (int)watch.ElapsedMilliseconds;
            Wait(waitTime);
        }
    }
    public static void Wait(long milliseconds)
    {
        if (milliseconds <= 0) return;
        Thread.Sleep((int)milliseconds);
    }
    public static Thread StartWindowThread(Game game)
    {
        var windowThread = new Thread(_ =>
        {
            Thread.CurrentThread.Name = "WINDOW";
            var windowProcess = new WindowProcess(game);
            windowProcess.Run();
        });
        windowThread.SetApartmentState(ApartmentState.STA);
        windowThread.Start();
        return windowThread;
    }
}