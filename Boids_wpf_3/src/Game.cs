using System.Numerics;
// using System.Runtime.InteropServices;

public static class Game
{
    private static int boidsCount = 100;
    private static Boid[] boids = new Boid[boidsCount];
    private static Boid[] boids2 = new Boid[boidsCount];
    private static AABB aabb = new AABB(Vector2.Zero, new Vector2(50,50));
    private static Calc a0 = new Calc(boids,aabb,0, 10,0);
    private static Calc a1 = new Calc(boids,aabb,10,10,1);
    private static Calc a2 = new Calc(boids,aabb,20,10,2);
    private static Calc a3 = new Calc(boids,aabb,30,10,3);
    private static Calc a4 = new Calc(boids,aabb,40,10,0);
    private static Calc a5 = new Calc(boids,aabb,50,10,1);
    private static Calc a6 = new Calc(boids,aabb,60,10,2);
    private static Calc a7 = new Calc(boids,aabb,70,10,3);
    private static Calc a8 = new Calc(boids,aabb,80,10,2);
    private static Calc a9 = new Calc(boids,aabb,90,10,3);
    public static unsafe void Init()
    {
        Subgen.Init(0);
        for (int i = 0; i < boidsCount; i++)
            boids[i] = new Boid(aabb);
    }
    public static void Update(bool debug)
    {
        // Parallel.For(0,boids.Length,i => { boids2[i] = Boid.UpdateVelocity(i,boids,false); });
        // boids2.CopyTo(boids,0);

        // for (int i = 0; i < boids.Length; i++)
        //     boids2[i] = Boid.UpdateVelocity(i,boids,debug);

        a0.Work(false);
        a1.Work(false);
        a2.Work(false);
        a3.Work(false);
        a4.Work(false);
        a5.Work(false);
        a6.Work(false);
        a7.Work(false);
        a8.Work(false);
        a9.Work(false);

        a0.Wait();
        a1.Wait();
        a2.Wait();
        a3.Wait();
        a4.Wait();
        a5.Wait();
        a6.Wait();
        a7.Wait();
        a8.Wait();
        a9.Wait();

        a0.Set();
        a1.Set();
        a2.Set();
        a3.Set();
        a4.Set();
        a5.Set();
        a6.Set();
        a7.Set();
        a8.Set();
        a9.Set();

        for (int i = 0; i < boids.Length; i++)
            Boid.UpdatePosition(ref boids[i], ref aabb, 0.02f);
    }
    public static void End()
    {
        a0.Finish();
        a1.Finish();
        a2.Finish();
        a3.Finish();
        a4.Finish();
        a5.Finish();
        a6.Finish();
        a7.Finish();
        a8.Finish();
        a9.Finish();

        Boid.Print(ref boids[0]);
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