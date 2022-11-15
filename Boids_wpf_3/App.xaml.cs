using System.Windows;
using System.Diagnostics;
using System.Windows.Media;

public partial class App : Application
{
    private static bool shutdownCalled = false;
    private static Thread thread2 = new Thread(Thread2);
    private static Stopwatch renderWatch = new Stopwatch();
    public App()
    {
        CompositionTarget.Rendering += OnRendering;
        Exit += OnExit;
        GameEngine.Init();
        Draw.Init();
        thread2 = new Thread(Thread2);
        thread2.Start();
    }
    private static void OnExit(object sender, ExitEventArgs e)
    {
        shutdownCalled = true;
    }
    private static void ManualShutdown()
    {
        Application.Current.Shutdown();
        shutdownCalled = true;
    }
    private static void OnRendering(object? sender, EventArgs e)
    {
        if (shutdownCalled) return;
        try
        {
            renderWatch.Restart();
            Draw.Render();
            renderWatch.Stop();
            var calcTime = renderWatch.ElapsedMilliseconds;
            if (calcTime > 15) // ~60FPS
                Console.WriteLine($"Rendering IS SLOW {calcTime}ms");
        }
        catch (System.Exception exc)
        {
            Console.WriteLine(exc.ToString());
            ManualShutdown();
        }
    }
    private static void Thread2()
    {
        try
        {
            var ticksPerMillisecond = Stopwatch.Frequency/1000;
            long timer = 0;
            var watch = Stopwatch.StartNew();

            while (true)
            {
                timer += watch.ElapsedTicks;
                watch.Restart();

                if (shutdownCalled)
                {
                    GameEngine.End();
                    return;
                }

                if (timer >= ticksPerMillisecond)
                {
                    var deltaTime = timer/ticksPerMillisecond;
                    timer %= ticksPerMillisecond;

                    GameEngine.Update(deltaTime);
                }
            }
        }
        catch (System.Exception exc)
        {
            Console.WriteLine(exc.ToString());
            ManualShutdown();
        }
    }
}