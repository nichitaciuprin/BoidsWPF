public unsafe static class Programm
{
    public unsafe static void Main()
    {
        System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.High;
        Thread.CurrentThread.Priority = ThreadPriority.Highest;

        Game.Init();

        var watch = System.Diagnostics.Stopwatch.StartNew();

        for (int i = 0; i < 2000; i++) Game.Update(false);

        watch.Stop();
        System.Console.WriteLine(watch.ElapsedMilliseconds);

        Game.End();
    }
}