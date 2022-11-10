using System.Numerics;
// using System.Runtime.InteropServices;

public static class Game
{
    private static int boidsCount = 100;
    private static Boid[] boids = new Boid[boidsCount];
    private static Boid[] boids2 = new Boid[boidsCount];
    private static AABB aabb = new AABB(Vector2.Zero, new Vector2(50,50));
    public static unsafe void Init()
    {
        Subgen.Init(0);
        for (int i = 0; i < boidsCount; i++)
            boids[i] = new Boid(aabb);
    }
    public static void Update(bool debug)
    {
        Parallel.For(0,boids.Length,i => { boids2[i] = Boid.UpdateVelocity(i,boids,false); });
        boids2.CopyTo(boids,0);

        // for (int i = 0; i < boids.Length; i++)
        //     boids2[i] = Boid.UpdateVelocity(i,boids,debug);

        // boids2.CopyTo(boids,0);

        for (int i = 0; i < boids.Length; i++)
            Boid.UpdatePosition(ref boids[i], ref aabb, 0.02f);
    }
    public static void End()
    {
        Boid.Print(ref boids[0]);
    }
}