﻿using System.Windows;
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
            var processID = int.Parse(e.Args[0]);
            callingProcess = Process.GetProcessById(processID);
            CompositionTarget.Rendering += OnRendering_Proc;
        }
        catch (System.Exception)
        {
            CompositionTarget.Rendering += OnRendering_NoProc;
        }

        try
        {
            GameEngine.Init();
            Draw.Init();
        }
        catch (System.Exception exc)
        {
            Console.WriteLine(exc);
            ManualShutdown();
        }

        thread2 = new Thread(SystemLoop);
        thread2.Start();
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
    private static void OnRendering_Proc(object? sender, EventArgs e)
    {
        if (callingProcess!.HasExited) ManualShutdown();
        if (shutdownCalled) return;
        OnRendering();
    }
    private static void OnRendering_NoProc(object? sender, EventArgs e)
    {
        if (shutdownCalled) return;
        OnRendering();
    }
    private static void OnRendering()
    {
        try
        {
            watch_render.Restart();
            Draw.Render();
            watch_render.Stop();
            Helper.MaybeWarn(nameof(OnRendering),watch_render.ElapsedMilliseconds,15);

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
                Thread.Sleep(30);
                // ADVANCED. Calc time only at the end.
                // if (GameEngine.MinTimer > 5)
                // {
                //     var timeout = (int)(GameEngine.MinTimer - 5);
                //     Thread.Sleep(timeout);
                // }

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
        }
        catch (System.Exception exc)
        {
            Console.WriteLine(exc.ToString());
            ManualShutdown();
        }
    }
}