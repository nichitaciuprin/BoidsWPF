using System.Numerics;

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
        var randDirection = Vector2Ext.RandNormDir();
        pos = aabb.RandPointInside;
        vel = Vector2Ext.Mul(randDirection,randSpeed);
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

            var diff = Vector2Ext.Sub(boid.pos,otherBoid.pos);
            var dist = Vector2Ext.Length(diff);

            // COHESION
            if (dist < range_1)
            {
                vec_1 = Vector2Ext.Add(vec_1,otherBoid.pos);
                count_1++;
            }

            // ALIGHMENT
            if (dist < range_2)
            {
                vec_2 = Vector2Ext.Add(vec_2,otherBoid.vel);
                count_2++;
            }

            // SEPARATION
            if (dist < range_3)
            {
                var normDiff = Vector2Ext.Normalize(diff);
                var d2 = range_3 - dist;
                normDiff = Vector2Ext.Mul(normDiff,d2);
                vec_3 = Vector2Ext.Add(vec_3,normDiff);
            }
        }

        if (count_1 != 0)
        {
            vec_1 = Vector2Ext.Div(vec_1,count_1);
            vec_1 = Vector2Ext.Sub(vec_1,boid.pos);
        }
        if (count_2 != 0)
        {
            vec_2 = Vector2Ext.Div(vec_2,count_2);
            vec_2 = Vector2Ext.Sub(vec_2,boid.vel);
        }

        vec_1 = Vector2Ext.Mul(vec_1,power1);
        vec_2 = Vector2Ext.Mul(vec_2,power2);
        vec_3 = Vector2Ext.Mul(vec_3,power3);

        var result = boid.vel;
        result = Vector2Ext.Add(Vector2Ext.Add(Vector2Ext.Add(result,vec_1),vec_2),vec_3);
        result = Vector2Ext.ClampLength(result,minSpeed,maxSpeed);
        boid.vel = result;
        return boid;
    }
    public static void UpdatePosition(ref Boid boid, ref AABB aabb, float deltaTime)
    {
        var velocityDelta = Vector2Ext.Mul(boid.vel,deltaTime);
        boid.pos = Vector2Ext.Add(boid.pos,velocityDelta);
        boid.pos = aabb.WrapAround(boid.pos);
    }
    public override string ToString()
    {
        return Vector2Ext.ToHex(pos) + Vector2Ext.ToHex(vel);
    }
}