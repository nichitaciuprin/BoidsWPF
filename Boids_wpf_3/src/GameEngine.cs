using System.Diagnostics;

public static class GameEngine
{
    private static long realTime = 0;
    private static long gameTime = 0;
    private static long frameCount = 0;
    private static long updateTimer = updateTimeStep;
    private static long fixedUpdateTimer = fixedUpdateTimeStep;
    private static Stopwatch updateWatch = new Stopwatch();
    private static Stopwatch fixedUpdateWatch = new Stopwatch();
    private static readonly long updateTimeStep = 15; // ~60FPS;
    private static readonly long fixedUpdateTimeStep = 20;

    public static void MainInit()
    {
        Init();
    }
    public static void MainEnd()
    {
        End();
    }
    public static void MainUpdate(long deltaTime)
    {
        if (deltaTime == 0) return;

        realTime += deltaTime;
        fixedUpdateTimer -= deltaTime;
        if (fixedUpdateTimer < 0)
        {
            deltaTime += fixedUpdateTimer;
            fixedUpdateTimer = 0;
        }
        updateTimer -= deltaTime;
        gameTime += deltaTime;

        Debug.Assert(deltaTime >= 0);

        if (updateTimer <= 0)
        {
            updateTimer = updateTimeStep;

            updateWatch.Restart();
            Update(updateTimeStep - updateTimer);
            updateWatch.Stop();

            var calcTime = updateWatch.ElapsedMilliseconds;
            if (calcTime > updateTimeStep)
                System.Console.WriteLine($"Update IS SLOW {calcTime}ms");

            frameCount++;
        }
        if (fixedUpdateTimer == 0)
        {
            fixedUpdateTimer = fixedUpdateTimeStep;

            fixedUpdateWatch.Restart();
            FixedUpdate(fixedUpdateTimeStep);
            fixedUpdateWatch.Stop();

            var calcTime = fixedUpdateWatch.ElapsedMilliseconds;
            if (calcTime > fixedUpdateTimeStep)
                System.Console.WriteLine($"FixedUpdate IS SLOW {calcTime}ms");
        }
    }

    private static void Init()
    {
        Game.Init();
    }
    private static void End()
    {
        Game.End();
    }
    private static void Update(long deltaTime)
    {
    }
    private static void FixedUpdate(long deltaTime)
    {
        Game.Update();
    }
}