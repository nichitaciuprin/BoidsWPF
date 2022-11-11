using MyVector2;

public struct Boid
{
    public Vector2 pos;
    public Vector2 vel;
    private Vector2 vec_1 = Vector2.Zero;
    private Vector2 vec_2 = Vector2.Zero;
    private Vector2 vec_3 = Vector2.Zero;
    private int count_1 = 0;
    private int count_2 = 0;

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
    public static void Update(Boid[] boids, ref AABB aabb)
    {
        Span<Boid> span1 = new Span<Boid>(boids);
        Span<Boid> span2 = stackalloc Boid[boids.Length];
        for (int i = 0; i < boids.Length; i++)
            span2[i] = Boid.UpdateVelocity(i,boids);
        span2.CopyTo(span1);

        // UpdateVelocity(boids);

        UpdatePosition(boids, ref aabb, 0.02f);
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
    public static void UpdatePosition(Boid[] boids, ref AABB aabb, float deltaTime)
    {
        for (int i = 0; i < boids.Length; i++)
        {
            var velocityDelta = Vector2.Mul(boids[i].vel,deltaTime);
            boids[i].pos = Vector2.Add(boids[i].pos,velocityDelta);
            boids[i].pos = aabb.WrapAround(boids[i].pos);
        }
    }
    public static void UpdateVelocity(Boid[] boids)
    {
        // ALL UNIQUE PAIRS
        var length = boids.Length;
        for (int i = 0;   i < length; i++)
        for (int j = i+1; j < length; j++)
            UpdateVelocity_1(ref boids[i], ref boids[j]);

        for (int i = 0;   i < length; i++)
            UpdateVelocity_2(ref boids[i]);
    }
    public static void UpdateVelocity_1(ref Boid boid1, ref Boid boid2)
    {
        var diff = Vector2.Sub(boid1.pos,boid2.pos);
        var dist = diff.Length();

        if (dist < range_1) return;

        // COHESION
        boid1.vec_1 = Vector2.Add(boid1.vec_1,boid2.pos); boid1.count_1++;
        boid2.vec_1 = Vector2.Add(boid2.vec_1,boid1.pos); boid2.count_1++;

        if (dist < range_2) return;

        // ALIGHMENT
        boid1.vec_2 = Vector2.Add(boid1.vec_2,boid2.vel); boid1.count_2++;
        boid2.vec_2 = Vector2.Add(boid2.vec_2,boid1.vel); boid2.count_2++;

        if (dist < range_3) return;

        // SEPARATION
        var normDiff = diff.Normalized();
        var d2 = range_3 - dist;
        normDiff = Vector2.Mul(normDiff,d2);
        boid1.vec_3 = Vector2.Add(boid1.vec_3,normDiff);
        boid2.vec_3 = Vector2.Add(boid2.vec_3,normDiff.Negate);
    }
    private static void UpdateVelocity_2(ref Boid boid)
    {
        if (boid.count_1 != 0)
        {
            boid.vec_1 = Vector2.Div(boid.vec_1,boid.count_1);
            boid.vec_1 = Vector2.Sub(boid.vec_1,boid.pos);
        }
        if (boid.count_2 != 0)
        {
            boid.vec_2 = Vector2.Div(boid.vec_2,boid.count_2);
            boid.vec_2 = Vector2.Sub(boid.vec_2,boid.vel);
        }

        boid.vec_1 = Vector2.Mul(boid.vec_1,power1);
        boid.vec_2 = Vector2.Mul(boid.vec_2,power2);
        boid.vec_3 = Vector2.Mul(boid.vec_3,power3);

        var result = boid.vel;
        result = Vector2.Add(Vector2.Add(Vector2.Add(result,boid.vec_1),boid.vec_2),boid.vec_3);
        result = result.ClampLength(minSpeed,maxSpeed);
        boid.vel = result;

        boid.vec_1 = Vector2.Zero;
        boid.vec_2 = Vector2.Zero;
        boid.vec_3 = Vector2.Zero;
        boid.count_1 = 0;
        boid.count_2 = 0;
    }
    public override string ToString()
    {
        return pos.ToHex() + vel.ToHex();
    }
}