using MyVector2;

public class GameState
{
    public AABB aabb;
    public Boid[] boids;
    public GameState(AABB aabb, Boid[] boids)
    {
        this.aabb = aabb;
        this.boids = boids;
    }
};
public static class Game
{
    public const int boidsCount = 300;
    public static GameState Init()
    {
        var aabb = new AABB(Vector2.Zero, new Vector2(50,50));
        var boids = new Boid[300];
        for (int i = 0; i < boids.Length; i++)
            boids[i] = new Boid(aabb);
        var gameState = new GameState(aabb,boids);
        return gameState;
    }
    public static void End(GameState gameState)
    {
    }
    public static void Update(GameState gameState, long deltaTime)
    {
        var deltaTimeInSeconds = ((float)deltaTime)/1000f;
        Boid.Update(gameState.boids, ref gameState.aabb, deltaTimeInSeconds);
    }
}