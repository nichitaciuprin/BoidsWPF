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
    private static readonly long updateTimeStep = Helper.ToTicks(15); // ~60FPS;
    private static readonly long fixedUpdateTimeStep = Helper.ToTicks(20);

    public static void MainInit()
    {
        Game.Init();
    }
    public static void MainEnd()
    {
        Game.End();
    }
    public static void MainUpdate(long deltaTime)
    {
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
        Debug.Assert(fixedUpdateTimer >= 0);

        if (updateTimer <= 0)
        {
            updateWatch.Restart();
            Update(updateTimeStep - updateTimer);
            updateWatch.Stop();

            var calcTime = updateWatch.ElapsedTicks;
            if (calcTime > updateTimeStep)
                System.Console.WriteLine($"Update IS SLOW {Helper.ToMilliseconds(calcTime)}ms");

            frameCount++;
            updateTimer = updateTimeStep;
        }
        if (fixedUpdateTimer == 0)
        {
            fixedUpdateWatch.Restart();
            FixedUpdate(fixedUpdateTimeStep);
            fixedUpdateWatch.Stop();

            fixedUpdateTimer = fixedUpdateTimeStep;

            var calcTime = fixedUpdateWatch.ElapsedTicks;
            if (calcTime > fixedUpdateTimeStep)
                System.Console.WriteLine($"FixedUpdate IS SLOW {Helper.ToMilliseconds(calcTime)}ms");
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