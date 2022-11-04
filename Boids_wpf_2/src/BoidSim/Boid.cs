using System;
using System.Numerics;
using System.Text.Json;

public class Boid
{
    public Vector2 pos;
    public Vector2 vel;
    private float minSpeed = 9f;
    private float maxSpeed = 15f;
    private float range_1 = 5f;
    private float range_2 = 5f;
    private float range_3 = 2f;
    private float range_4 = 10f;
    private float power1 = .01f;
    private float power2 = .01f;
    private float power3 = .04f;
    private float power4 = .05f;

    public Boid(Vector2 pos, Vector2 vel) => (this.pos,this.vel) = (pos,vel);
    public Boid(Random rand, AABB worldSpace)
    {
        var size = worldSpace.Size;
        var x = Helper.RandFloat(rand,0,size.X);
        var y = Helper.RandFloat(rand,0,size.Y);
        pos = new Vector2(x,y);
        vel = Vector2Ext.RandomVectorNorm(rand) * minSpeed;
    }
    public static void Serialise(Boid[] boids)
    {
        var str = JsonSerializer.Serialize<Boid[]>(boids);
        // var lines = boid.Select(x => string.Join(',',x.pos.X,x.pos.Y,x.vel.X,x.vel.Y)).ToArray();
        var path = AppDomain.CurrentDomain.BaseDirectory+@"..\..\..\boids.txt";
        if (System.IO.File.Exists(path))
            System.IO.File.Delete(path);
        System.IO.File.WriteAllText(path,str);
    }
    public void UpdateVelocity_boids(Boid[] boids, Boid[] predators)
    {
        var vec_1 = Vector2.Zero;
        var vec_2 = Vector2.Zero;
        var vec_3 = Vector2.Zero;
        var vec_4 = Vector2.Zero;
        var count_1 = 0;
        var count_2 = 0;

        for (int i = 0; i < boids.Length; i++)
        {
            var otherBoid = boids[i];

            if (this == otherBoid) continue;

            var diff = this.pos-otherBoid.pos;
            var dist = diff.Length();

            // COHESION
            if (dist < range_1)
            {
                vec_1 += otherBoid.pos;
                count_1++;
            }

            // ALIGHMENT
            if (dist < range_2)
            {
                vec_2 += otherBoid.vel;
                count_2++;
            }

            // SEPARATION
            if (dist < range_3)
            {
                vec_3 += Vector2.Normalize(diff) * (range_3 - dist);
            }
        }

        for (int i = 0; i < predators.Length; i++)
        {
            var otherBoid = predators[i];

            var diff = this.pos-otherBoid.pos;
            var dist = diff.Length();

            // PREDATORS
            if (dist < range_4)
            {
                vec_4 += Vector2.Normalize(diff) * (range_4 - diff.Length());
            }
        }

        if (count_1 != 0) vec_1 = vec_1/count_1 - this.pos;
        if (count_2 != 0) vec_2 = vec_2/count_2 - this.vel;
        vel += vec_1 * power1;
        vel += vec_2 * power2;
        vel += vec_3 * power3;
        vel += vec_4 * power4;
        vel = vel.ClampLength(minSpeed,maxSpeed);
    }
    public void UpdateVelocity_predators(Boid[] boids, Boid[] predators)
    {
        var vec_1 = Vector2.Zero;
        var vec_2 = Vector2.Zero;
        var vec_3 = Vector2.Zero;
        var count_1 = 0;
        var count_2 = 0;

        for (int i = 0; i < predators.Length; i++)
        {
            var otherBoid = boids[i];

            if (this == otherBoid) continue;

            var diff = this.pos-otherBoid.pos;
            var dist = diff.Length();

            // COHESION
            if (dist < range_1)
            {
                vec_1 += otherBoid.pos;
                count_1++;
            }

            // ALIGHMENT
            if (dist < range_2)
            {
                vec_2 += otherBoid.vel;
                count_2++;
            }

            // SEPARATION
            if (dist < range_3)
            {
                vec_3 += Vector2.Normalize(diff) * (range_3 - diff.Length());
            }
        }

        if (count_1 != 0) vec_1 = vec_1/count_1 - this.pos;
        if (count_2 != 0) vec_2 = vec_2/count_2 - this.vel;
        vel += vec_1 * power1;
        vel += vec_2 * power2;
        vel += vec_3 * power3;
        vel = vel.ClampLength(minSpeed,maxSpeed);
    }
    public void UpdatePosition(AABB worldSpace, float deltaTime)
    {
        pos += vel * deltaTime;
        pos = worldSpace.WrapAround(pos);
    }
}