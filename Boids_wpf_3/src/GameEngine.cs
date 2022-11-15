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
    private static readonly long fixedUpdateTimeStep = 10;

    public static void Init()
    {
        Game.Init();
    }
    public static void End()
    {
        Game.End();
    }
    public static void Update(long deltaTime)
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
            Game.Update(updateTimeStep - updateTimer);
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
            Game.FixedUpdate(fixedUpdateTimeStep);
            fixedUpdateWatch.Stop();

            var calcTime = fixedUpdateWatch.ElapsedMilliseconds;
            if (calcTime > fixedUpdateTimeStep)
                System.Console.WriteLine($"FixedUpdate IS SLOW {calcTime}ms");
        }
    }
}