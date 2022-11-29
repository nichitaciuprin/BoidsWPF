using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

public class MyWindow
{
    public Window window;
    private Canvas canvas;
    private Queue<Polygon> triangles;
    private Queue<Line> lines;
    private int width = 500;
    private int height = 500;
    private int scale = 10;

    public MyWindow(string title)
    {
        window = new Window();
        window.WindowStyle = WindowStyle.None;
        window.ResizeMode = ResizeMode.NoResize;
        window.AllowsTransparency = false;
        window.Title = title;
        window.Left = 0;
        window.Top = 0;
        window.Width = width;
        window.Height = height;
        window.Background = Brushes.Magenta;

        canvas = new Canvas();
        canvas.Background = Brushes.Black;
        canvas.Width = window.Width;
        canvas.Height = window.Height;
        window.Content = canvas;

        window.Show();

        var poligonsArray = (new bool[1000])
            .Select(x => new Polygon())
            .Select(x =>
            {
                x.Points.Add(new Point());
                x.Points.Add(new Point());
                x.Points.Add(new Point());
                canvas.Children.Add(x);
                return x;
            })
            .ToArray();
        triangles = new Queue<Polygon>(poligonsArray);

        var linesArray = (new bool[10])
            .Select(x =>
            {
                var line = new Line();
                canvas.Children.Add(line);
                return line;
            })
            .ToArray();
        lines = new Queue<Line>(linesArray);
    }
    public void BeginDraw()
    {
        Clear();
    }
    public void EndDraw()
    {
    }
    public void DrawRectangleLines(int posX, int posY, int width, int height)
    {
        var p0 = new MyVector2(posX, posY);
        var p1 = new MyVector2(posX, posY + height);
        var p2 = new MyVector2(posX + width, posY + height);
        var p3 = new MyVector2(posX + width, posY);
        DrawLine(p0,p1);
        DrawLine(p1,p2);
        DrawLine(p2,p3);
        DrawLine(p3,p0);
    }
    public void DrawTriangle(MyVector2 v1, MyVector2 v2, MyVector2 v3, Brush brush)
    {
        v1 = ToWindowSpace(v1);
        v2 = ToWindowSpace(v2);
        v3 = ToWindowSpace(v3);
        var triangle = GetTriangle();
        triangle.Points[0] = new Point(v1.x,v1.y);
        triangle.Points[1] = new Point(v2.x,v2.y);
        triangle.Points[2] = new Point(v3.x,v3.y);
        triangle.Visibility = Visibility.Visible;
        triangle.Fill = brush;
    }
    private void DrawLine(MyVector2 start, MyVector2 end)
    {
        var line = GetLine();
        start = ToWindowSpace(start);
        end = ToWindowSpace(end);
        line.X1 = start.x;
        line.Y1 = start.y;
        line.X2 = end.x;
        line.Y2 = end.y;
        line.Stroke = Brushes.DarkCyan;
        line.StrokeThickness = 1;
        line.Visibility = Visibility.Visible;
    }
    private MyVector2 ToWindowSpace(MyVector2 v)
    {
        v = MyVector2.Mul(v, scale);
        v.y = height - v.y;
        return new MyVector2(v.x, v.y);
    }
    private void Clear()
    {
        var uiItems = canvas.Children;
        for (int i = 0; i < uiItems.Count; i++)
            uiItems[i].Visibility = Visibility.Hidden;
    }
    private Polygon GetTriangle()
    {
        var result = triangles.Dequeue();
        triangles.Enqueue(result);
        return result;
    }
    private Line GetLine()
    {
        var result = lines.Dequeue();
        lines.Enqueue(result);
        return result;
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
    private void Test_Polygons()
    {
        var scale = 20;
        for (int i = 0; i < scale; i++)
        for (int j = 0; j < scale; j++)
        {
            var x = i*20;
            var y = j*20;
            var scale2 = 5;
            var polygon = new Polygon();
            polygon.Fill = Brushes.Green;
            polygon.Points.Add(new Point(x+0,y+0));
            polygon.Points.Add(new Point(x+0,y+scale2));
            polygon.Points.Add(new Point(x+scale2,y+scale2));
            polygon.Points.Add(new Point(x+scale2,y+0));
            canvas.Children.Add(polygon);
        }
    }
}