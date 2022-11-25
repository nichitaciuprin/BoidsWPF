using MyVector2;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Controls;

public class MyWindow
{
    public float scale = 12f;
    public Window window;
    private Canvas canvas;

    public MyWindow(string title)
    {
        window = new Window();
        window.WindowStyle = WindowStyle.None;
        window.ResizeMode = ResizeMode.NoResize;
        window.AllowsTransparency = false;
        window.Title = title;
        window.Left = 0;
        window.Top = 0;
        var size = new Vector2(600,600);
        window.Width = size.x;
        window.Height = size.y;
        window.Background = Brushes.Magenta;

        canvas = new Canvas();
        canvas.Background = Brushes.Black;
        canvas.Width = window.Width;
        canvas.Height = window.Height;
        window.Content = canvas;

        window.Show();
        window.KeyDown += OnKeyDown;
    }
    public void BeginDraw()
    {
        Clear();
    }
    public void EndDraw()
    {
    }
    public MyPolygon Create_MyPolygon(Vector2[] points)
    {
        points = points.Select(x => { x.y = -x.y; return x; } ).ToArray();
        points = points.Select(x => Vector2.Mul(x,scale)).ToArray();
        var myPolygon = new MyPolygon(points);
        for (int i = 0; i < points.Length; i++)
            myPolygon.polygon.Points.Add(new Point(points[i].x,points[i].y));
        canvas.Children.Add(myPolygon.polygon);
        myPolygon.polygon.Visibility = Visibility.Visible;
        return myPolygon;
    }
    public Line Create_Line()
    {
        var line = new Line();
        canvas.Children.Add(line);
        return line;
    }
    public void Draw_MyPolygon(MyPolygon myPolygon, Vector2 pos, Vector2 angle, Brush color)
    {
        // var polygonPoints = myPolygon.polygon.Points;
        // for (int i = 0; i < polygonPoints.Count; i++)
        // {
        //     var point = myPolygon.points[i];
        //     point.x *= scale;
        //     point.y *= scale;
        //     var point2 = polygonPoints[i];
        //     point2.X = point.x;
        //     point2.Y = point.y;
        //     polygonPoints[i] = point2;
        // }
        myPolygon.polygon.Fill = color;
        myPolygon.myTransform.Set(myPolygon.polygon,ToWindowPosition(pos),GetAngle(angle));
        myPolygon.polygon.Visibility = Visibility.Visible;
    }
    public void Draw_DrawLine(Line line, Vector2 start, Vector2 end)
    {
        start = ToWindowPosition(start);
        end = ToWindowPosition(end);
        line.X1 = start.x;
        line.Y1 = start.y;
        line.X2 = end.x;
        line.Y2 = end.y;
        line.Stroke = Brushes.DarkCyan;
        line.StrokeThickness = 1;
        line.Visibility = Visibility.Visible;
    }
    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Escape:
                // Shutdown();
                break;
            default:
                break;
        }
    }
    private void Clear()
    {
        var coll = canvas.Children;
        for (int i = 0; i < coll.Count; i++)
        {
            var item = coll[i];
            item.Visibility = Visibility.Hidden;
        }
    }
    private Vector2 ToWindowPosition(Vector2 worldPos) => new Vector2(worldPos.x*scale, (float)canvas.Height - worldPos.y*scale);
    private float GetAngle(Vector2 vel)
    {
        if (Vector2.IsZero(vel)) return 0;
        return MathF.Atan2(vel.x,vel.y) * 180f / MathF.PI;
    }
    private void Test_CreateWindowBorder()
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
        {var line = new Line();
        line.X1 = 0;
        line.Y1 = 0;
        line.X2 = canvas.Width;
        line.Y2 = canvas.Height;
        line.StrokeThickness = 1;
        line.Stroke = Brushes.Red;
        canvas.Children.Add(line);}

        {var line = new Line();
        line.X1 = canvas.Width;
        line.Y1 = 0;
        line.X2 = 0;
        line.Y2 = canvas.Height;
        line.StrokeThickness = 1;
        line.Stroke = Brushes.Red;
        canvas.Children.Add(line);}
    }
    public class MyPolygon
    {
        internal MyTransform myTransform = new MyTransform();
        internal Polygon polygon = new Polygon();
        internal Vector2[] points;
        internal MyPolygon(Vector2[] points)
        {
            this.points = points;
        }
    }
    internal class MyTransform
    {
        internal TransformGroup tg;
        internal TranslateTransform tt;
        internal RotateTransform rt;

        internal MyTransform()
        {
            tg = new TransformGroup();
            tt = new TranslateTransform();
            rt = new RotateTransform();
            tg.Children.Add(rt);
            tg.Children.Add(tt);
        }
        internal void Set(Shape shape, Vector2 pos, float rot)
        {
            tt.X = pos.x;
            tt.Y = pos.y;
            rt.Angle = rot;
            shape.RenderTransform = tg;
        }
    }
}