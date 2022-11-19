using System.Diagnostics;

public static class GameEngine
{
    private static long realTime = 0;
    private static long gameTime = 0;
    private static long frameCount = 0;
    private static long timer_fixedUpdate = timestep_fixedUpdate;
    private static Stopwatch watch_update = new Stopwatch();
    private static Stopwatch watch_fixedUpdate = new Stopwatch();
    private const long timestep_fixedUpdate = 20;

    public static long MinTimer => timer_fixedUpdate;

    public static GameState gameState;
    public static GameState gameState_forRender;

    public static void Init()
    {
        gameState = Game.Init();
        gameState_forRender = new GameState();
        gameState_forRender.boids = new Boid[gameState.boids.Length];
        gameState_forRender.aabb = gameState.aabb;
        Copy(ref gameState, ref gameState_forRender);
    }
    public static void End()
    {
        Game.End(ref gameState);
    }
    public static void Loop(long deltaTime)
    {
        if (deltaTime == 0) return;
        realTime += deltaTime;
        timer_fixedUpdate -= deltaTime;
        if (timer_fixedUpdate < 0)
        {
            deltaTime += timer_fixedUpdate;
            timer_fixedUpdate = 0;
        }
        gameTime += deltaTime;

#if DEBUG
        Debug.Assert(deltaTime >= 0);
#endif

        if (timer_fixedUpdate == 0)
        {
            timer_fixedUpdate = timestep_fixedUpdate;
            watch_fixedUpdate.Restart();
            Game.Update(ref gameState, timestep_fixedUpdate);
            Copy(ref gameState, ref gameState_forRender);
            watch_fixedUpdate.Stop();
            Helper.MaybeWarn(nameof(Game.Update),watch_fixedUpdate.ElapsedMilliseconds,timestep_fixedUpdate);
        }
        else
        {
            Game.Update(ref gameState_forRender, deltaTime);
        }

        frameCount++;
    }
    public static void Copy(ref GameState logicState, ref GameState renderState)
    {
        renderState.aabb = logicState.aabb;
        var lenght = logicState.boids.Length;
        for (int i = 0; i < lenght; i++)
            renderState.boids[i] = logicState.boids[i];
    }
}