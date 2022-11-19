using MyVector2;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Controls;

public class WindowProcess : Application
{
    private GameState gameState;
    private float scale = 12f;
    private Window window;
    private Canvas canvas;
    private Queue<MyTransform> myTransformPool;
    private Queue<Shape> boidShapePool;

    public WindowProcess(GameState gameState)
    {
        this.gameState = gameState;

        window = CreateWindow();
        canvas = CreateCanvas();
        myTransformPool = CreateMyTransformPool(Game.boidsCount);
        boidShapePool = CreateBoidShapePool(Game.boidsCount);

        CreateBorder();
        // Test_Elipse();
        // Test_Square();
        // Test_BoidPolygon();
        // Test_Cross();
        Render(gameState);
        window.Show();
        window.KeyDown += OnKeyDown;
        Startup += OnStartup;
        Exit += OnExit;
        CompositionTarget.Rendering += OnRendering;
    }
    private void OnStartup(object sender, StartupEventArgs e)
    {
    }
    private void OnExit(object sender, ExitEventArgs e)
    {
    }
    private void OnRendering(object? sender, EventArgs e)
    {
        try
        {
            Render(gameState);
        }
        catch (System.Exception exc)
        {
            Console.WriteLine(exc.ToString());
            Application.Current.Shutdown();
        }
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
    private Window CreateWindow()
    {
        var window = new Window();
        window.WindowStyle = WindowStyle.None;
        window.ResizeMode = ResizeMode.NoResize;
        window.AllowsTransparency = false;
        window.Title = "TestWPF";
        window.Left = 0;
        window.Top = 0;
        var size = new Vector2(600,600);
        window.Width = size.x;
        window.Height = size.y;
        window.Background = Brushes.Magenta;
        return window;
    }
    private Canvas CreateCanvas()
    {
        var canvas = new Canvas();
        canvas.Background = Brushes.Black;
        canvas.Width = window.Width;
        canvas.Height = window.Height;
        window.Content = canvas;
        return canvas;
    }
    private Queue<MyTransform> CreateMyTransformPool(int size)
    {
        return new Queue<MyTransform>((new bool[size]).Select(x => new MyTransform()));
    }
    private Queue<Shape> CreateBoidShapePool(int size)
    {
        var boidShapePool =  new Queue<Shape>((new bool[size]).Select(x => CreateBoidShape()));
        foreach (var item in boidShapePool)
            canvas.Children.Add(item);
        return boidShapePool;
    }
    private void Render(GameState gameState)
    {
        HideAll();
        Render(gameState.boids);
    }
    private void CreateBorder()
    {
        var size = 2;
        var color = Brushes.DarkCyan;

        {
        var topBar = new Rectangle();
        topBar.Width = canvas.Width;
        topBar.Height = size;
        topBar.Fill = color;
        canvas.Children.Add(topBar);
        Canvas.SetZIndex(topBar,1);
        Canvas.SetTop(topBar,0);
        }

        {
        var bottomBar = new Rectangle();
        bottomBar.Width = canvas.Width;
        bottomBar.Height = size;
        bottomBar.Fill = color;
        canvas.Children.Add(bottomBar);
        Canvas.SetZIndex(bottomBar,1);
        Canvas.SetBottom(bottomBar,0);
        }

        {
        var leftBar = new Rectangle();
        leftBar.Width = size;
        leftBar.Height = canvas.Height;
        leftBar.Fill = color;
        canvas.Children.Add(leftBar);
        Canvas.SetZIndex(leftBar,1);
        Canvas.SetLeft(leftBar,0);
        }

        {
        var rightBar = new Rectangle();
        rightBar.Width = size;
        rightBar.Height = canvas.Height;
        rightBar.Fill = color;
        canvas.Children.Add(rightBar);
        Canvas.SetZIndex(rightBar,1);
        Canvas.SetRight(rightBar,0);
        }
    }
    private void Test_Elipse()
    {
        var shape = new Ellipse();
        shape.Height = 50;
        shape.Width = 50;
        shape.Fill = Brushes.Red;
        canvas.Children.Add(shape);
        var newTrans = new MyTransform();
        newTrans.Set(shape,new Vector2((float)-shape.Width/2,(float)-shape.Height/2),0);
    }
    private void Test_Cross()
    {
        var brush = new SolidColorBrush();
        brush.Color = Colors.Red;

        {var line = new Line();
        line.X1 = 0;
        line.Y1 = 0;
        line.X2 = canvas.Width;
        line.Y2 = canvas.Height;
        line.StrokeThickness = 1;
        line.Stroke = brush;
        canvas.Children.Add(line);}

        {var line = new Line();
        line.X1 = canvas.Width;
        line.Y1 = 0;
        line.X2 = 0;
        line.Y2 = canvas.Height;
        line.StrokeThickness = 1;
        line.Stroke = brush;
        canvas.Children.Add(line);}
    }
    private void Test_Square()
    {
        var brush = new SolidColorBrush();
        brush.Color = Colors.Green;

        var p0 = new Vector2(0,0);
        var p1 = new Vector2(0,100);
        var p2 = new Vector2(100,100);
        var p3 = new Vector2(100,0);

        var line0 = new Line(); line0.Stroke = brush; line0.StrokeThickness = 1; canvas.Children.Add(line0); SetPosition(line0,p0,p1);
        var line1 = new Line(); line1.Stroke = brush; line1.StrokeThickness = 1; canvas.Children.Add(line1); SetPosition(line1,p1,p2);
        var line2 = new Line(); line2.Stroke = brush; line2.StrokeThickness = 1; canvas.Children.Add(line2); SetPosition(line2,p2,p3);
        var line3 = new Line(); line3.Stroke = brush; line3.StrokeThickness = 1; canvas.Children.Add(line3); SetPosition(line3,p3,p0);
    }
    private void Test_BoidPolygon()
    {
        var shape = CreateBoidShape();
        shape.Fill = Brushes.Green;
        var tran = new MyTransform();
        tran.Set(shape,new Vector2((float)canvas.Width/2f,(float)canvas.Height/2),90);
        canvas.Children.Add(shape);
    }
    private void Render(AABB aabb)
    {
        // var length = 4;
        // for (int i = 0; i < length; i++)
        // {
        // }
    }
    private void Render(Boid[] boids)
    {
        if (boids.Length == 0) return;
        RenderBoid(boids,0,Brushes.Red);
        for (int i = 1; i < boids.Length; i++)
            RenderBoid(boids,i,Brushes.White);
    }
    private void RenderBoid(Boid[] boids, int i, SolidColorBrush color)
    {
        var boid = boids[i];
        var shape = boidShapePool.Dequeue(); boidShapePool.Enqueue(shape);
        var myTransform = myTransformPool.Dequeue(); myTransformPool.Enqueue(myTransform);
        shape.Fill = color;
        shape.Visibility = Visibility.Visible;
        SetPosition(boid,myTransform,shape);
    }
    private Shape CreateBoidShape()
    {
        var shape = new Polygon();
        var points = new []
        {
            new Point(0, 0),
            new Point(-4, 1),
            new Point(0, -8),
            new Point(4, 1),
            new Point(0, 0),
        };
        var duno = 1f/8f/2f*scale;
        for (int i = 0; i < points.Length; i++)
            points[i] = new Point(points[i].X*duno,points[i].Y*duno);
        for (int i = 0; i < points.Length; i++)
            shape.Points.Add(points[i]);
        return shape;
    }
    private void HideAll()
    {
        foreach (var item in boidShapePool) item.Visibility = Visibility.Hidden;
        // foreach (var item in linePool) item.Visibility = Visibility.Hidden;
    }
    private void SetPosition(Boid boid, MyTransform myTransform, Shape boidShape)
    {
        myTransform.Set(boidShape,ToRenderPos(boid.pos),GetAngle(boid.vel));
    }
    private float GetAngle(Vector2 vel)
    {
        if (Vector2.IsZero(vel)) return 0;
        return MathF.Atan2(vel.x,vel.y) * 180f / MathF.PI;
    }
    private void SetPosition(Line line, Vector2 worldPosition_Start, Vector2 worldPosition_End)
    {
        var offsetY = (float)canvas.Height;
        line.X1 = worldPosition_Start.x;
        line.Y1 = offsetY - worldPosition_Start.y;
        line.X2 = worldPosition_End.x;
        line.Y2 = offsetY - worldPosition_End.y;
    }
    private Vector2 ToRenderPos(Vector2 worldPos) => (new Vector2(worldPos.x*scale,(float)canvas.Height - worldPos.y*scale));
    private class MyTransform
    {
        private TransformGroup tg;
        private TranslateTransform tt;
        private RotateTransform rt;

        public MyTransform()
        {
            tg = new TransformGroup();
            tt = new TranslateTransform();
            rt = new RotateTransform();
            tg.Children.Add(rt);
            tg.Children.Add(tt);
        }
        public void Set(Shape shape, Vector2 pos, float rot)
        {
            tt.X = pos.x;
            tt.Y = pos.y;
            rt.Angle = rot;
            shape.RenderTransform = tg;
        }
    }
}