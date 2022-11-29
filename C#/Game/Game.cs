public class Game
{
    public AABB aabb;
    public Boid[] boids = new Boid[300];

    public Game()
    {
        var subgen = new Subgen(0);
        aabb = new AABB(MyVector2.Zero, new MyVector2(50,50));
        for (int i = 0; i < boids.Length; i++)
            boids[i] = new Boid(aabb,subgen);
    }
    public static void Update(Game gameState, long deltaTime)
    {
        var deltaTimeInSeconds = ((float)deltaTime)/1000f;
        Boid.Update(gameState.boids, ref gameState.aabb, deltaTimeInSeconds);
    }
}