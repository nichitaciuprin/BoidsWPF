using MyVector2;

public struct Boid
{
    public Vector2 pos;
    public Vector2 vel;
    private static float minSpeed = 9f;
    private static float maxSpeed = 15f;
    private static float range_1 = 5f;
    private static float range_2 = 5f;
    private static float range_3 = 2f;
    private static float power1 = .01f;
    private static float power2 = .01f;
    private static float power3 = .04f;

    public Boid(AABB aabb)
    {
        var randSpeed = Subgen.Range(minSpeed,maxSpeed);
        var randDirection = Vector2.RandNormDir();
        pos = aabb.RandPointInside;
        vel = Vector2.Mul(randDirection,randSpeed);
    }
    public static void Update(Boid[] boids, Boid[] boids2, ref AABB aabb)
    {
        // Span<Boid> localBoids = stackalloc Boid[boids.Length];

        for (int i = 0; i < boids.Length; i++)
            boids2[i] = Boid.UpdateVelocity(i,boids);

        boids2.CopyTo(boids,0);

        for (int i = 0; i < boids.Length; i++)
            Boid.UpdatePosition(ref boids[i], ref aabb, 0.02f);
    }
    public static Boid UpdateVelocity(int thisBoidIndex, Boid[] boids)
    {
        var vec_1 = Vector2.Zero;
        var vec_2 = Vector2.Zero;
        var vec_3 = Vector2.Zero;
        var count_1 = 0;
        var count_2 = 0;

        var boid = boids[thisBoidIndex];
        for (int i = 0; i < boids.Length; i++)
        {
            var otherBoid = boids[i];

            if (thisBoidIndex == i) continue;

            var diff = Vector2.Sub(boid.pos,otherBoid.pos);
            var dist = diff.Length();

            // COHESION
            if (dist < range_1)
            {
                vec_1 = Vector2.Add(vec_1,otherBoid.pos);
                count_1++;
            }

            // ALIGHMENT
            if (dist < range_2)
            {
                vec_2 = Vector2.Add(vec_2,otherBoid.vel);
                count_2++;
            }

            // SEPARATION
            if (dist < range_3)
            {
                var normDiff = diff.Normalized();
                var d2 = range_3 - dist;
                normDiff = Vector2.Mul(normDiff,d2);
                vec_3 = Vector2.Add(vec_3,normDiff);
            }
        }

        if (count_1 != 0)
        {
            vec_1 = Vector2.Div(vec_1,count_1);
            vec_1 = Vector2.Sub(vec_1,boid.pos);
        }
        if (count_2 != 0)
        {
            vec_2 = Vector2.Div(vec_2,count_2);
            vec_2 = Vector2.Sub(vec_2,boid.vel);
        }

        vec_1 = Vector2.Mul(vec_1,power1);
        vec_2 = Vector2.Mul(vec_2,power2);
        vec_3 = Vector2.Mul(vec_3,power3);

        var result = boid.vel;
        result = Vector2.Add(Vector2.Add(Vector2.Add(result,vec_1),vec_2),vec_3);
        result = result.ClampLength(minSpeed,maxSpeed);
        boid.vel = result;
        return boid;
    }
    public static void UpdatePosition(ref Boid boid, ref AABB aabb, float deltaTime)
    {
        var velocityDelta = Vector2.Mul(boid.vel,deltaTime);
        boid.pos = Vector2.Add(boid.pos,velocityDelta);
        boid.pos = aabb.WrapAround(boid.pos);
    }
    public override string ToString()
    {
        return pos.ToHex() + vel.ToHex();
    }
}