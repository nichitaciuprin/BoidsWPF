using System.Numerics;
// using System.Runtime.InteropServices;

public static class Game
{
    private static int boidsCount = 2;
    private static Boid[] boids = new Boid[boidsCount];
    private static AABB aabb = new AABB(Vector2.Zero, new Vector2(50,50));
    private static Calc a1 = new Calc(boids,aabb,0,1,1);
    private static Calc a2 = new Calc(boids,aabb,1,1,2);
    // private static Calc a3 = new Calc(boids,50,25);
    // private static Calc a4 = new Calc(boids,75,25);
    public static unsafe void Init()
    {
        Subgen.Init(0);
        for (int i = 0; i < boidsCount; i++)
            boids[i] = new Boid(aabb);
    }
    public static void Update(bool debug)
    {
        // Parallel.For(0,boids.Length,x => { Boid.UpdateVelocity(x,boids); });

        // for (int i = 0; i < boids.Length; i++)
        //     Boid.UpdateVelocity(i,boids,debug);
        // for (int i = 0; i < boids.Length; i++)
        //     Boid.UpdatePosition(ref boids[i], ref aabb, 0.02f);

        // var oldBoid = boids[0];
        // Boid.Print(ref boids[0]);
        a1.Set();
        while(a1.working) { Thread.Yield(); }
        a2.Set();
        while(a2.working) { Thread.Yield(); }
        // System.Console.Write(',');
        // var newBoid = boids[0];
    }
    public static void End()
    {
        a1.Finish();
        a2.Finish();
        // a3.Finish();
        // a4.Finish();
        // Boid.Print(ref boids[0]);
        for (int i = 0; i < boids.Length; i++)
            Boid.Print(ref boids[i]);
    }
}
public class Calc
{
    private int startIndex;
    private int length;
    private Boid[] boids;
    private AABB aabb;
    private Thread thread;
    public bool working;
    private bool finish;
    public int count;
    private int id;
    public Calc(Boid[] boids, AABB aabb, int startIndex, int length, int id)
    {
        this.id = id;
        this.boids = boids;
        this.aabb = aabb;
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
                count++;
                var length2 = startIndex+length;
                for (int i = startIndex; i < length2; i++)
                    Boid.UpdateVelocity(i,boids,false);
                for (int i = startIndex; i < length2; i++)
                    Boid.UpdatePosition(ref boids[i], ref aabb, 0.02f);
                working = false;
            }
            Thread.Yield();
        }
    }
    // public static object lockObj = new();
}