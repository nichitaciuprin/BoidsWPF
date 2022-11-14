using System.Diagnostics;

public abstract class WPFChildThread
{
    private bool abort = false;
    private Thread thread;
    private Stopwatch loopWatch = new Stopwatch();

    public WPFChildThread()
    {
        thread = new Thread(ThreadStart);
    }
    public void Start()
    {
        thread.Start();
    }
    public void End()
    {
        abort = true;
    }
    private void ThreadStart()
    {
        try
        {
            while (true)
            {
                if (abort) break;
                GameEngine.MainUpdate(loopWatch.ElapsedTicks);
                loopWatch.Restart();
            }
        }
        catch (System.Exception)
        {
            // TODO
            abort = true;
            throw;
        }
    }
}