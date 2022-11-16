using System.Windows;
using System.Diagnostics;
using System.Windows.Media;

public partial class App : Application
{
    private static bool shutdownCalled = false;
    private static long ticksPerMillisecond = Stopwatch.Frequency/1000;
    private static long timer_systemLoop = 0;
    private static Stopwatch watch_render = Stopwatch.StartNew();
    private static Stopwatch watch_realTime = Stopwatch.StartNew();
    // private static Thread thread2 = new Thread(SystemLoop);
    public App()
    {
        CompositionTarget.Rendering += OnRendering;
        Exit += OnExit;
        GameEngine.Init();
        Draw.Init();
        // thread2 = new Thread(SystemLoop);
        // thread2.Start();
    }
    private static void OnExit(object sender, ExitEventArgs e)
    {
        shutdownCalled = true;
        GameEngine.End();
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
            watch_render.Restart();
            Draw.Render();
            watch_render.Stop();
            Helper.MaybeWarn(nameof(OnRendering),watch_render.ElapsedMilliseconds,15);

            timer_systemLoop += watch_realTime.ElapsedTicks;
            watch_realTime.Restart();
            if (timer_systemLoop >= ticksPerMillisecond)
            {
                var deltaTime = timer_systemLoop/ticksPerMillisecond;
                timer_systemLoop %= ticksPerMillisecond;
                // Helper.MaybeWarn("SystemDeltaTime",deltaTime,15);
                GameEngine.Loop(deltaTime);
            }
        }
        catch (System.Exception exc)
        {
            Console.WriteLine(exc.ToString());
            ManualShutdown();
        }
    }
    // private static void SystemLoop()
    // {
    //     try
    //     {
    //         while (true)
    //         {
    //             if (shutdownCalled)
    //             {
    //                 GameEngine.End();
    //                 return;
    //             }

    //             // LOW FPS BUT LOW CPU USAGE
    //             // Thread.Sleep(10);
    //             // if (GameEngine.MinTimer > 5)
    //             // {
    //             //     var timeout = (int)(GameEngine.MinTimer - 5);
    //             //     Thread.Sleep(timeout);
    //             // }

    //             timer_systemLoop += watch_systemLoop.ElapsedTicks;
    //             watch_systemLoop.Restart();
    //             if (timer_systemLoop >= ticksPerMillisecond)
    //             {
    //                 var deltaTime = timer_systemLoop/ticksPerMillisecond;
    //                 timer_systemLoop %= ticksPerMillisecond;
    //                 // Helper.MaybeWarn(nameof(SystemLoop),deltaTime,15);
    //                 GameEngine.Loop(deltaTime);
    //             }
    //         }
    //     }
    //     catch (System.Exception exc)
    //     {
    //         Console.WriteLine(exc.ToString());
    //         ManualShutdown();
    //     }
    // }
}