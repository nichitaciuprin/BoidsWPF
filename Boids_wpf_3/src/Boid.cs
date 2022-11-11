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
        var dist1 = diff.Length();

        if (dist1 >= range_1) return;

        // COHESION
        boid1.vec_1 = Vector2.Add(boid1.vec_1,boid2.pos); boid1.count_1++;
        boid2.vec_1 = Vector2.Add(boid2.vec_1,boid1.pos); boid2.count_1++;

        if (dist1 >= range_2) return;

        // ALIGHMENT
        boid1.vec_2 = Vector2.Add(boid1.vec_2,boid2.vel); boid1.count_2++;
        boid2.vec_2 = Vector2.Add(boid2.vec_2,boid1.vel); boid2.count_2++;

        if (dist1 >= range_3) return;

        // SEPARATION
        var normDiff = diff.Normalized();
        var dist2 = range_3 - dist1;
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