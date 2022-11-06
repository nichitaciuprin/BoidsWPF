using System.Numerics;
// using System.Runtime.InteropServices;

public static class Game
{
    private static int boidsCount = 100;
    private static Boid[] boids = new Boid[boidsCount];
    private static AABB aabb = new AABB(Vector2.Zero, new Vector2(50,50));
    // private static Calc a1 = new Calc(boids,0, 25);
    // private static Calc a2 = new Calc(boids,24,25);
    // private static Calc a3 = new Calc(boids,49,25);
    // private static Calc a4 = new Calc(boids,74,25);
    public static unsafe void Init()
    {
        Subgen.Init(0);
        for (int i = 0; i < boidsCount; i++)
            boids[i] = new Boid(aabb);
        // Boid.Print(ref boids[0]);
    }
    public static void Update()
    {
        // Parallel.For(0,boids.Length,x => { Boid.UpdateVelocity(x,boids); });

        for (int i = 0; i < boids.Length; i++)
            Boid.UpdateVelocity(i,boids);

        // a1.Set();
        // a2.Set();
        // a3.Set();
        // a4.Set();
        // while(a1.working) Thread.Yield();
        // while(a2.working) Thread.Yield();
        // while(a3.working) Thread.Yield();
        // while(a4.working) Thread.Yield();

        for (int i = 0; i < boids.Length; i++)
            Boid.UpdatePosition(ref boids[i], ref aabb, 0.02f);
    }
    public static void End()
    {
        // a1.Finish();
        // a2.Finish();
        // a3.Finish();
        // a4.Finish();
        Boid.Print(ref boids[0]);
    }
}
public class Calc
{
    private int startIndex;
    private int length;
    private Boid[] boids;
    private Thread thread;
    public bool working;
    private bool finish;
    public Calc(Boid[] boids, int startIndex, int length)
    {
        this.boids = boids;
        this.startIndex = startIndex;
        this.length = length;
        this.thread = new Thread(Duno);
        thread.Start();
    }
    public void Set()
    {
        working = true;
    }
    public void Finish()
    {
        finish = true;
    }
    public void Duno()
    {
        while (true)
        {
            if (finish) return;
            if (working)
            {
                for (int i = startIndex; i < length; i++)
                    Boid.UpdateVelocity(i,boids);
                working = false;
            }
            Thread.Yield();
        }
    }
}