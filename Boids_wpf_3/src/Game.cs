using MyVector2;

public static class Game
{
    private static int boidsCount = 300;
    private static Boid[] boids = new Boid[boidsCount];
    private static AABB aabb = new AABB(Vector2.Zero, new Vector2(50,50));
    public static unsafe void Init()
    {
        Subgen.Init(0);
        for (int i = 0; i < boidsCount; i++)
            boids[i] = new Boid(aabb);
    }
    public static void Update()
    {
        Boid.Update(boids,ref aabb,0.02f);
    }
    public static void End()
    {
        System.Console.WriteLine(boids[0].ToString());
    }
}