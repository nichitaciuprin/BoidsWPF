using System;
using System.Numerics;

public class BoidSim : GameEngine
{
    private const int seed = 0;
    private static readonly Random rand = new Random(seed);
    public AABB boidsWorldSpace = new AABB(Vector2.Zero,Vector2.One*50);
    public Boid[] boids = new Boid[500];
    public Boid[] predators = new Boid[1];

    public BoidSim()
    {
        for (int i = 0; i < boids.Length; i++) boids[i] = new Boid(rand, boidsWorldSpace);
        // Boid.Serialise(boids);
        for (int i = 0; i < predators.Length; i++) predators[i] = new Boid(rand, boidsWorldSpace);
    }
    protected override void Update(long deltaTime)
    {
    }
    protected override void FixedUpdate(long deltaTime)
    {
        var deltaTimeF = deltaTime.Seconds();
        // Helper.Start();
        for (int i = 0; i < boids.Length; i++) boids[i].UpdateVelocity_boids(boids,predators);
        for (int i = 0; i < predators.Length; i++) predators[i].UpdateVelocity_predators(boids,predators);
        // Helper.End();
        for (int i = 0; i < boids.Length; i++) boids[i].UpdatePosition(boidsWorldSpace,deltaTimeF);
        for (int i = 0; i < predators.Length; i++) predators[i].UpdatePosition(boidsWorldSpace,deltaTimeF);
    }
}