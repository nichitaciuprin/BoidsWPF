using MyVector2;

public static class Game
{
    public static int boidsCount = 300;
    public static Boid[] boids = new Boid[boidsCount];
    public static AABB aabb = new AABB(Vector2.Zero, new Vector2(50,50));
    public static void Init()
    {
        for (int i = 0; i < boidsCount; i++)
            boids[i] = new Boid(aabb);
    }
    public static void End()
    {
        System.Console.Write($"{boids[0].ToString()} ");
    }
    public static void Update(long deltaTime)
    {
    }
    public static void FixedUpdate(long deltaTime)
    {
        var deltaTimeInSeconds = ((float)deltaTime)/1000f;
        Boid.Update(boids, ref aabb, deltaTimeInSeconds);
    }
}