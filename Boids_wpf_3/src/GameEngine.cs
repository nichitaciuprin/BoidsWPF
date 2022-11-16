using System.Diagnostics;

public static class GameEngine
{
    private static long realTime = 0;
    private static long gameTime = 0;
    private static long frameCount = 0;
    private static long timer_update = timestep_update;
    private static long timer_fixedUpdate = timestep_fixedUpdate;
    private static Stopwatch watch_update = new Stopwatch();
    private static Stopwatch watch_fixedUpdate = new Stopwatch();
    private static readonly long timestep_update = 15; // ~60FPS;
    private static readonly long timestep_fixedUpdate = 20;

    public static long MinTimer => System.Math.Min(timer_update,timer_fixedUpdate);

    public static void Init()
    {
        Game.Init();
    }
    public static void End()
    {
        Game.End();
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
        timer_update -= deltaTime;
        gameTime += deltaTime;
        Debug.Assert(deltaTime >= 0);

        if (timer_update <= 0)
        {
            timer_update = timestep_update;
            watch_update.Restart();
            Game.Update(timestep_update - timer_update);
            watch_update.Stop();
            Helper.MaybeWarn(nameof(Game.Update),watch_update.ElapsedMilliseconds,timestep_update);

            frameCount++;
        }
        if (timer_fixedUpdate == 0)
        {
            timer_fixedUpdate = timestep_fixedUpdate;
            watch_fixedUpdate.Restart();
            Game.FixedUpdate(timestep_fixedUpdate);
            watch_fixedUpdate.Stop();
            Helper.MaybeWarn(nameof(Game.FixedUpdate),watch_fixedUpdate.ElapsedMilliseconds,timestep_fixedUpdate);
        }
    }
}