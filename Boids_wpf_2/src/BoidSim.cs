using MyVector2;

public class BoidSim : GameEngine
{
    public int boidsCount = 300;
    public Boid[] boids;
    public AABB aabb = new AABB(Vector2.Zero, new Vector2(50,50));

    public BoidSim()
    {
        Subgen.Init(0);
        boids = new Boid[boidsCount];
        for (int i = 0; i < boidsCount; i++)
            boids[i] = new Boid(aabb);
    }
    protected override void Update(long deltaTime)
    {
    }
    protected override void FixedUpdate(long deltaTime)
    {
        var deltaTimeF = deltaTime.Seconds();
        Boid.Update(boids,ref aabb,0.02f);
    }
}