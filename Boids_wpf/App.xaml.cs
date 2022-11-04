using System;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;

public partial class App : Application
{
    private static bool shutdownCalled = false;
    private static Stopwatch renderingWatch = new Stopwatch();
    private static BoidSim boidSim = new BoidSim();
    private static BoidSimRendering boidSimRendering = new BoidSimRendering(boidSim);
    public App()
    {
        Loging.ClearFileIfExist();
        CompositionTarget.Rendering += OnRendering;
        Exit += OnExit;
        boidSim.Start();
    }
    private static void OnExit(object sender, ExitEventArgs e)
    {
        boidSim.End();
        Loging.CreateFileLogs();
    }
    private static void OnRendering(object? sender, EventArgs e)
    {
        if (shutdownCalled) return;
        if (boidSimRendering == null) return;
        if (boidSim.GameTime.Seconds() >= 10) { ManualShutdown(); return; }
        try
        {
            renderingWatch.Restart();
            boidSimRendering.Render();
            renderingWatch.Stop();
            var calcTime = renderingWatch.ElapsedTicks;
            if (Helper.ToMilliseconds(calcTime) > 15) // ~60FPS
                Loging.WriteLine($"Rendering IS SLOW {Helper.ToMilliseconds(calcTime)}ms");
        }
        catch (System.Exception exc)
        {
            Loging.WriteLine(exc.ToString());
            ManualShutdown();
        }
    }
    private static void ManualShutdown()
    {
        Application.Current.Shutdown();
        shutdownCalled = true;
    }
}