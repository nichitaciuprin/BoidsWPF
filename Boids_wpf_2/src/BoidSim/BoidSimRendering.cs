using System.Linq;
using System.Collections.Generic;
using System.Numerics;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using System;

public class BoidSimRendering
{
    private float scale = 12f;
    private Window window;
    private Canvas canvas;
    private BoidSim model;
    private Queue<MyTransform> myTransformPool;
    private Queue<Shape> boidShapePool;
    // private Queue<Line> linePool;

    public BoidSimRendering(BoidSim model)
    {
        this.model = model;
        window = CreateWindow();
        canvas = CreateCanvas();
        CreateBorder();
        // Test_Elipse();
        // Test_Square();
        // Test_BoidPolygon();
        // Test_Cross();

        var size2 = model.boids.Length + model.predators.Length;
        myTransformPool = new Queue<MyTransform>((new bool[size2]).Select(x => new MyTransform()));
        boidShapePool = new Queue<Shape>((new bool[size2]).Select(x => CreateBoidShape()));
        foreach (var item in boidShapePool)
            canvas.Children.Add(item);
        // linePool = (new object[0]).Select(x => new Line()).ToArray();
        // foreach (var item in linePool)      canvas.Children.Add(item);

        Render();
        window.Show();
    }
    public void Render()
    {
        HideAll();
        Render(model.boids,Brushes.White);
        Render(model.predators,Brushes.Red);
    }
    private Window CreateWindow()
    {
        window = new Window();
        window.WindowStyle = WindowStyle.None;
        window.ResizeMode = ResizeMode.NoResize;
        window.AllowsTransparency = false;
        window.Title = "TestWPF";
        window.Left = 0;
        window.Top = 0;
        var size = new Vector2(600,600);
        window.Width = size.X;
        window.Height = size.Y;
        window.Background = Brushes.Magenta;
        return window;
    }
    private Canvas CreateCanvas()
    {
        canvas = new Canvas();
        canvas.Background = Brushes.Black;
        canvas.Width = window.Width;
        canvas.Height = window.Height;
        window.Content = canvas;
        return canvas;
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
    private void Render(Boid[] boids, SolidColorBrush color)
    {
        for (int i = 0; i < boids.Length; i++)
        {
            var boid = boids[i];
            var shape = boidShapePool.Dequeue(); boidShapePool.Enqueue(shape);
            var myTransform = myTransformPool.Dequeue(); myTransformPool.Enqueue(myTransform);
            shape.Fill = color;
            shape.Visibility = Visibility.Visible;
            SetPosition(boid,myTransform,shape);
        }
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
    public float GetAngle(Vector2 vel)
    {
        if (vel == Vector2.Zero) return 0;
        return MathF.Atan2(vel.X,vel.Y) * 180f / MathF.PI;
    }
    private void SetPosition(Line line, Vector2 worldPosition_Start, Vector2 worldPosition_End)
    {
        var offsetY = (float)canvas.Height;
        line.X1 = worldPosition_Start.X;
        line.Y1 = offsetY - worldPosition_Start.Y;
        line.X2 = worldPosition_End.X;
        line.Y2 = offsetY - worldPosition_End.Y;
    }
    private Vector2 ToRenderPos(Vector2 worldPos) => (new Vector2(worldPos.X*scale,(float)canvas.Height - worldPos.Y*scale));
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
            tt.X = pos.X;
            tt.Y = pos.Y;
            rt.Angle = rot;
            shape.RenderTransform = tg;
        }
    }
}