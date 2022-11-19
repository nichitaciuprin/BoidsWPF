using System.Diagnostics;

public static class Program
{
    [STAThread]
    public static void Main()
    {
        var curentProcess = Process.GetCurrentProcess();
        var curentThread = Thread.CurrentThread;
        curentProcess.PriorityClass = ProcessPriorityClass.High;
        curentThread.Name = "MAIN";
        curentThread.Priority = ThreadPriority.Highest;

        var gameState = Game.Init();
        var watch = new Stopwatch();
        var timeStep = 20;
        var windowThread = StartWindowThread(gameState);
        while (windowThread.IsAlive)
        {
            watch.Restart();
            Game.Update(gameState, timeStep);
            watch.Stop();
            var waitTime = timeStep - (int)watch.ElapsedMilliseconds;
            Wait(waitTime);
        }
        Game.End(gameState);
    }
    public static void Wait(long milliseconds)
    {
        if (milliseconds <= 0) return;
        Thread.Sleep((int)milliseconds);
    }
    public static Thread StartWindowThread(GameState gameState)
    {
        var windowThread = new Thread(_ =>
        {
            Thread.CurrentThread.Name = "WINDOW";
            var windowProcess = new WindowProcess(gameState);
            windowProcess.Run();
        });
        windowThread.SetApartmentState(ApartmentState.STA);
        windowThread.Start();
        return windowThread;
    }
}