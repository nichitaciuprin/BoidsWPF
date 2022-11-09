using System.Numerics;
// using System.Runtime.InteropServices;

public static class Game
{
    private static int boidsCount = 100;
    private static Boid[] boids = new Boid[boidsCount];
    private static Boid[] boids2 = new Boid[boidsCount];
    private static AABB aabb = new AABB(Vector2.Zero, new Vector2(50,50));
    private static Calc a1 = new Calc(boids,aabb,0, 25,0);
    private static Calc a2 = new Calc(boids,aabb,25,25,1);
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
        //     boids2[i] = Boid.UpdateVelocity(i,boids,debug);

        // if (debug)
        // {
        //     for (int i = 0; i < boids.Length; i++)
        //         Boid.Print(ref boids2[i]);
        // }

        // boids2.CopyTo(boids,0);
        // for (int i = 0; i < boids.Length; i++)
        //     Boid.UpdatePosition(ref boids[i], ref aabb, 0.02f);

        // var oldBoid = boids[0];
        // Boid.Print(ref boids[0]);

        a1.Fetch();
        a2.Fetch();

        a1.Work(debug);
        a2.Work(debug);
        a1.Wait();
        a2.Wait();

        a1.Set();
        a2.Set();

        for (int i = 0; i < boids.Length; i++)
            Boid.UpdatePosition(ref boids[i], ref aabb, 0.02f);
    }
    public static void End()
    {
        a1.Finish();
        a2.Finish();
        // a3.Finish();
        // a4.Finish();
        Boid.Print(ref boids[0]);
        // for (int i = 0; i < boids.Length; i++)
        //     Boid.Print(ref boids[i]);
    }
}
public class Calc
{
    private int startIndex;
    private int length;
    private Boid[] boids;
    private Boid[] localBoids;
    private AABB aabb;
    private Thread thread;
    public bool working;
    private bool finish;
    public int count;
    private int id;
    private bool debug;
    public Calc(Boid[] boids, AABB aabb, int startIndex, int length, int id)
    {
        this.id = id;
        this.boids = boids;
        this.aabb = aabb;
        this.startIndex = startIndex;
        this.length = length;
        this.thread = new Thread(Duno);
        this.localBoids = new Boid[boids.Length];
        thread.Start();
    }
    public void Fetch()
    {
        // Array.Copy(boids,localBoids,boids.Length);
        // boids.CopyTo(localBoids,0);
    }
    public void Work(bool debug)
    {
        working = true;
        this.debug = debug;
    }
    public void Wait()
    {
        while(working) { Thread.Yield(); }
    }
    public void Set()
    {
        Array.ConstrainedCopy(localBoids,startIndex,boids,startIndex,length);
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
                var length2 = startIndex+length;
                for (int i = startIndex; i < length2; i++)
                    localBoids[i] = Boid.UpdateVelocity(i,boids,debug);
                // for (int i = startIndex; i < length2; i++)
                working = false;
            }
            Thread.Yield();
        }
    }
    public static object lockObj = new();
}