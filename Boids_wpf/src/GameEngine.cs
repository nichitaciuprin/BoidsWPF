using System.Diagnostics;
using System.Threading;

public abstract class GameEngine
{
    private bool abort = false;
    private Thread thread;
    private Stopwatch loopWatch = new Stopwatch();
    private Stopwatch udpateWatch = new Stopwatch();
    private Stopwatch fixedUpdateWatch = new Stopwatch();
    private long frameCount = 0;
    private long gameTime = 0;
    private long deltaTime = 0;
    private readonly long updateTimeStep = Helper.ToTicks(15); // ~60FPS
    private readonly long fixedUpdateTimeStep = Helper.ToTicks(10);
    private long updateTimer = 0;
    private long fixedUpdateTimer = 0;

    public GameEngine()
    {
        thread = new Thread(ThreadStart);
    }
    public long GameTime => gameTime;
    public void Start()
    {
        thread.Start();
    }
    public void End()
    {
        abort = true;
    }
    protected abstract void Update(long deltaTime);
    protected abstract void FixedUpdate(long deltaTime);
    private void ThreadStart()
    {
        try
        {
            // MAYBE REMOVE. Wait to avoid init spike
            var initWatch = Stopwatch.StartNew();
            while (initWatch.ElapsedMilliseconds < 100)
                Thread.Yield();

            Loop();
        }
        catch (System.Exception)
        {
            // TODO
            abort = true;
            throw;
        }
    }
    private void Loop()
    {
        while (true)
        {
            if (abort) return;

            loopWatch.Restart();

            if (updateTimer <= 0)
            {
                updateTimer = updateTimeStep;
                udpateWatch.Restart();
                Update(deltaTime);
                frameCount++;
                udpateWatch.Stop();
                var calcTime = udpateWatch.ElapsedTicks;
                if (calcTime > updateTimeStep)
                    Loging.WriteLine($"Update IS SLOW {Helper.ToMilliseconds(calcTime)}ms");
            }
            if (fixedUpdateTimer == 0)
            {
                fixedUpdateTimer = fixedUpdateTimeStep;
                fixedUpdateWatch.Restart();
                FixedUpdate(fixedUpdateTimeStep);
                fixedUpdateWatch.Stop();
                var calcTime = fixedUpdateWatch.ElapsedTicks;
                if (calcTime > fixedUpdateTimeStep)
                    Loging.WriteLine($"FixedUpdate IS SLOW {Helper.ToMilliseconds(calcTime)}ms");
            }

            var waitTime = System.Math.Min(updateTimer,fixedUpdateTimer);
            while (true)
            {
                deltaTime = loopWatch.ElapsedTicks;
                if (deltaTime >= waitTime)
                    break;
                Thread.Yield();
            }
            if (deltaTime > fixedUpdateTimer)
                deltaTime = fixedUpdateTimer;

            gameTime += deltaTime;
            updateTimer -= deltaTime;
            fixedUpdateTimer -= deltaTime;

            Debug.Assert(fixedUpdateTimer >= 0);
        }
    }
}