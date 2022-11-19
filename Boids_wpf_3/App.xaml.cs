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
    private static Thread thread2 = new Thread(SystemLoop);
    private static Process? callingProcess;

    public App()
    {
        Startup += OnStartup;
        Exit += OnExit;
    }
    public static void ManualShutdown()
    {
        Application.Current.Shutdown();
        shutdownCalled = true;
    }
    private static void OnStartup(object sender, StartupEventArgs e)
    {
        try
        {
            GameEngine.Init();
            Draw.Init(ref GameEngine.gameState_forRender);
        }
        catch (System.Exception exc)
        {
            Console.WriteLine(exc);
            ManualShutdown();
        }
        try
        {
            var processID = int.Parse(e.Args[0]);
            callingProcess = Process.GetProcessById(processID);
            CompositionTarget.Rendering += (object? sender, EventArgs e) =>
            {
                if (callingProcess!.HasExited) ManualShutdown();
                OnRendering();
            };
        }
        catch (System.Exception)
        {
            CompositionTarget.Rendering += (object? sender, EventArgs e) =>
            {
                OnRendering();
            };
        }
        // thread2.Start();
    }
    private static void OnExit(object sender, ExitEventArgs e)
    {
        try
        {
            shutdownCalled = true;
            GameEngine.End();
        }
        catch (System.Exception exc)
        {
            Console.WriteLine(exc);
        }
    }
    private static void OnRendering()
    {
        if (shutdownCalled) return;
        try
        {
            timer_systemLoop += watch_realTime.ElapsedTicks;
            watch_realTime.Restart();
            if (timer_systemLoop >= ticksPerMillisecond)
            {
                var deltaTime = timer_systemLoop/ticksPerMillisecond;
                timer_systemLoop %= ticksPerMillisecond;
                // Helper.MaybeWarn("SystemDeltaTime",deltaTime,15);
                GameEngine.Loop(deltaTime);
            }
            // watch_render.Restart();
            Draw.Render(ref GameEngine.gameState_forRender);
            // Draw.Render(ref GameEngine.gameState_forRender);
            // watch_render.Stop();
            // Helper.MaybeWarn(nameof(OnRendering),watch_render.ElapsedMilliseconds,15);
        }
        catch (System.Exception exc)
        {
            Console.WriteLine(exc.ToString());
            ManualShutdown();
        }
    }
    private static void SystemLoop()
    {
        try
        {
            while (true)
            {
                if (shutdownCalled) return;

                // WILL LOWER CPU USAGE BUT ALSO FPS
                // SIMPLE.
                // Thread.Sleep(300);
                // ADVANCED. Calc time only at the end.
                // if (GameEngine.MinTimer > 5)
                // {
                //     var timeout = (int)(GameEngine.MinTimer - 5);
                //     Thread.Sleep(timeout);
                // }

                // timer_systemLoop += watch_realTime.ElapsedTicks;
                // watch_realTime.Restart();
                // if (timer_systemLoop >= ticksPerMillisecond)
                // {
                //     var deltaTime = timer_systemLoop/ticksPerMillisecond;
                //     timer_systemLoop %= ticksPerMillisecond;
                //     // Helper.MaybeWarn("SystemDeltaTime",deltaTime,15);
                //     GameEngine.Loop(deltaTime);
                // }
            }
        }
        catch (System.Exception exc)
        {
            Console.WriteLine(exc.ToString());
            ManualShutdown();
        }
    }
}