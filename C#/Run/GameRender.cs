using MyVector2;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Shapes;

public class GameRender : Application
{
    private Game game;
    private MyWindow myWindow;
    private MyWindow.MyPolygon[] myPoligons;
    private Line[] linePool;

    private Vector2 boidPoint_1 = new Vector2 ( 0.00f, 0.00f);
    private Vector2 boidPoint_2 = new Vector2 (-0.25f,-0.25f);
    private Vector2 boidPoint_3 = new Vector2 ( 0.00f, 0.50f);
    private Vector2 boidPoint_4 = new Vector2 ( 0.25f,-0.25f);

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
    public GameRender(Game game)
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

        myPoligons = game.boids.Select(x => myWindow.Create_MyPolygon(points)).ToArray();
        linePool = (new bool[100]).Select(x => myWindow.Create_Line()).ToArray();

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

    public void Render()
    {
        myWindow.BeginDraw();
        Render(game.boids);
        Render(game.aabb);
        myWindow.EndDraw();
    }
    private void Render(AABB aabb)
    {
        var p0 = new Vector2(aabb.MinX,aabb.MinY);
        var p1 = new Vector2(aabb.MinX,aabb.MaxY);
        var p2 = new Vector2(aabb.MaxX,aabb.MaxY);
        var p3 = new Vector2(aabb.MaxX,aabb.MinY);
        var line0 = linePool[0];
        var line1 = linePool[1];
        var line2 = linePool[2];
        var line3 = linePool[3];
        myWindow.Draw_DrawLine(line0,p0,p1);
        myWindow.Draw_DrawLine(line1,p1,p2);
        myWindow.Draw_DrawLine(line2,p2,p3);
        myWindow.Draw_DrawLine(line3,p3,p0);
    }
    private void Render(Boid[] boids)
    {
        if (boids.Length == 0) return;
        RenderBoid(ref boids[0],0,Brushes.Red);
        for (int i = 1; i < boids.Length; i++)
            RenderBoid(ref boids[i],i,Brushes.White);
    }
    private void RenderBoid(ref Boid boid, int i, Brush color)
    {
        myWindow.Draw_MyPolygon(myPoligons[i],boid.pos,boid.vel,color);
    }
}