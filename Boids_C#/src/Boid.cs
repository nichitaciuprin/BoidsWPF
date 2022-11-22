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

    private const float minSpeed = 9f;
    private const float maxSpeed = 15f;
    private const float range_1 = 5f;
    private const float range_2 = 5f;
    private const float range_3 = 2f;
    private const float rangeSquared_1 = range_1*range_1;
    private const float rangeSquared_2 = range_2*range_2;
    private const float rangeSquared_3 = range_3*range_3;
    private const float power1 = .01f;
    private const float power2 = .01f;
    private const float power3 = .04f;

    public Boid(AABB aabb, Subgen subgen)
    {
        var randSpeed = subgen.Range(minSpeed,maxSpeed);
        var randDirection = Vector2.RandNormDir(subgen);
        pos = aabb.RandPointInside(subgen);
        vel = Vector2.Mul(randDirection,randSpeed);
    }
    public static void Update(Boid[] boids, ref AABB aabb, float deltaTime)
    {
        var length = boids.Length;

        // ALL UNIQUE PAIRS
        for (int i = 0;   i < length; i++)
        for (int j = i+1; j < length; j++)
            UpdateVelocity_1(ref boids[i], ref boids[j]);

        for (int i = 0;   i < length; i++)
            UpdateVelocity_2(ref boids[i]);

        for (int i = 0; i < length; i++)
            UpdatePosition(ref boids[i], ref aabb,deltaTime);
    }
    private static void UpdateVelocity_1(ref Boid boid1, ref Boid boid2)
    {
        var diff = Vector2.Sub(boid1.pos,boid2.pos);
        var distSquared = diff.x*diff.x + diff.y*diff.y;

        // COHESION
        if (distSquared >= rangeSquared_1) return;

        boid1.vec_1 = Vector2.Add(boid1.vec_1,boid2.pos); boid1.count_1++;
        boid2.vec_1 = Vector2.Add(boid2.vec_1,boid1.pos); boid2.count_1++;

        // ALIGHMENT
        if (distSquared >= rangeSquared_2) return;

        boid1.vec_2 = Vector2.Add(boid1.vec_2,boid2.vel); boid1.count_2++;
        boid2.vec_2 = Vector2.Add(boid2.vec_2,boid1.vel); boid2.count_2++;

        // SEPARATION
        if (distSquared >= rangeSquared_3) return;

        var dist = MathF.Sqrt(distSquared);
        var normFraction = 1.0f/dist;
        var normDiff = new Vector2(diff.x*normFraction, diff.y*normFraction);
        var dist2 = range_3 - dist;
        normDiff = Vector2.Mul(normDiff,dist2);

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

        boid.vel = Vector2.Add(boid.vel,boid.vec_1);
        boid.vel = Vector2.Add(boid.vel,boid.vec_2);
        boid.vel = Vector2.Add(boid.vel,boid.vec_3);

        boid.vel = boid.vel.ClampLength(minSpeed,maxSpeed);

        boid.vec_1 = Vector2.Zero;
        boid.vec_2 = Vector2.Zero;
        boid.vec_3 = Vector2.Zero;
        boid.count_1 = 0;
        boid.count_2 = 0;
    }
    private static void UpdatePosition(ref Boid boid, ref AABB aabb, float deltaTime)
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