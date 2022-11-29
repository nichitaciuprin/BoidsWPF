using System.Windows;
using System.Windows.Media;
using System.Windows.Input;

public class GameRender : Application
{
    private Game game;
    private MyWindow myWindow;
    private MyVector2 boidPoint_1 = new MyVector2 ( 0.00f, 0.00f);
    private MyVector2 boidPoint_2 = new MyVector2 (-0.25f,-0.25f);
    private MyVector2 boidPoint_3 = new MyVector2 ( 0.00f, 0.50f);
    private MyVector2 boidPoint_4 = new MyVector2 ( 0.25f,-0.25f);

    public static Thread Start(Game game)
    {
        var windowThread = new Thread(_ =>
        {
            Thread.CurrentThread.Name = "GameRender";
            var windowProcess = new GameRender(game);
            windowProcess.Run();
        });
        windowThread.SetApartmentState(ApartmentState.STA);
        windowThread.Start();
        return windowThread;
    }
    private GameRender(Game game)
    {
        this.game = game;
        this.myWindow = new MyWindow("BOIDS_C#");

        var points = new []
        {
            boidPoint_1,
            boidPoint_2,
            boidPoint_3,
            boidPoint_4,
        };

        Render();

        Startup += OnStartup;
        Exit += OnExit;
        myWindow.window.KeyDown += OnKeyDown;
        CompositionTarget.Rendering += OnRendering;
    }
    private void OnStartup(object sender, StartupEventArgs e)
    {
    }
    private void OnExit(object sender, ExitEventArgs e)
    {
    }
    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Escape:
                Shutdown();
                break;
            default:
                break;
        }
    }
    private void OnRendering(object? sender, EventArgs e)
    {
        try
        {
            Render();
        }
        catch (System.Exception exc)
        {
            Console.WriteLine(exc.ToString());
            Application.Current.Shutdown();
        }
    }
    private void Render()
    {
        myWindow.BeginDraw();

        var boids = game.boids;
        if (boids.Length > 0)
        {
            Render(ref boids[0],Brushes.Red);
            for (int i = 1; i < boids.Length; i++)
                Render(ref boids[i],Brushes.White);
        }

        Render(ref game.aabb);

        myWindow.EndDraw();
    }
    private void Render(ref AABB aabb)
    {
        var pos = new MyVector2(aabb.MinX, aabb.MinY);
        var size = aabb.Size;
        myWindow.DrawRectangleLines((int)pos.x,(int)pos.y,(int)size.x,(int)size.y);
    }
    private void Render(ref Boid boid, Brush brush)
    {
        MyVector2 v1 = boidPoint_1;
        MyVector2 v2 = boidPoint_2;
        MyVector2 v3 = boidPoint_3;
        MyVector2 v4 = boidPoint_4;

        float angle = MyVector2.Angle(boid.vel);

        v1 = MyVector2.Rotate(v1, angle);
        v2 = MyVector2.Rotate(v2, angle);
        v3 = MyVector2.Rotate(v3, angle);
        v4 = MyVector2.Rotate(v4, angle);

        v1 = MyVector2.Add(boid.pos, v1);
        v2 = MyVector2.Add(boid.pos, v2);
        v3 = MyVector2.Add(boid.pos, v3);
        v4 = MyVector2.Add(boid.pos, v4);

        myWindow.DrawTriangle(v1, v2, v3, brush);
        myWindow.DrawTriangle(v1, v4, v3, brush);
    }
}