using MyVector2;

public struct GameState
{
    public AABB aabb;
    public Boid[] boids;
};
public static class Game
{
    public const int boidsCount = 300;
    public static GameState Init()
    {
        GameState gameState;
        gameState.aabb = new AABB(Vector2.Zero, new Vector2(50,50));
        gameState.boids = new Boid[300];
        for (int i = 0; i < gameState.boids.Length; i++)
            gameState.boids[i] = new Boid(gameState.aabb);
        return gameState;
    }
    public static void End(ref GameState gameState)
    {
        System.Console.WriteLine($"{gameState.boids[0].ToString()}");
    }
    public static void Update(ref GameState gameState, long deltaTime)
    {
        var deltaTimeInSeconds = ((float)deltaTime)/1000f;
        Boid.Update(gameState.boids, ref gameState.aabb, deltaTimeInSeconds);
    }
}