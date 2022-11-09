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
        float randSpeed = Subgen.Range(minSpeed,maxSpeed);
        Vector2 randDirection = Helper.RandNormDir();
        this.pos = aabb.RandPointInside;
        this.vel = Helper.Mul(randDirection,randSpeed);
    }
    public static Boid UpdateVelocity(int thisBoidIndex, Boid[] boids, bool debug)
    {
        Vector2 vec_1 = Vector2.Zero;
        Vector2 vec_2 = Vector2.Zero;
        Vector2 vec_3 = Vector2.Zero;
        int count_1 = 0;
        int count_2 = 0;

        var boid = boids[thisBoidIndex];
        // if (debug)
        // {
        //     Helper.PrintVector2Hex(boid.velocity);
        //     Boid.Print(ref boid);
        //     System.Console.WriteLine();
        // }
        for (int i = 0; i < boids.Length; i++)
        {
            var otherBoid = boids[i];

            if (thisBoidIndex == i) continue;

            // if (debug)
            // {
            //     var msg = $"{Helper.ToHex(otherBoid.velocity)}";
            //     System.Console.WriteLine(msg);
            // }

            var diff = Helper.Sub(boid.pos,otherBoid.pos);
            var dist = Helper.Length(diff);

            // if (debug)
            // {
            //     var msg = $"{Helper.ToHex(diff)} = {Helper.ToHex(boid.pos)} - {Helper.ToHex(otherBoid.pos)}";
            //     System.Console.WriteLine(msg);
            // }

            // COHESION
            if (dist < range_1)
            {
                vec_1 = Helper.Add(vec_1,otherBoid.pos);
                count_1++;
            }

            // ALIGHMENT
            if (dist < range_2)
            {
                vec_2 = Helper.Add(vec_2,otherBoid.vel);
                count_2++;
            }

            // SEPARATION
            if (dist < range_3)
            {
                var normDiff = Helper.Normalize(diff);
                var d2 = range_3 - dist;
                normDiff = Helper.Mul(normDiff,d2);
                vec_3 = Helper.Add(vec_3,normDiff);
            }
        }

        // if (debug)
        // {
        //     var msg = $"{Helper.ToHex(vec_2)}";
        //     System.Console.WriteLine(msg);
        // }

        if (count_1 != 0)
        {
            vec_1 = Helper.Div(vec_1,count_1);
            vec_1 = Helper.Sub(vec_1,boid.pos);
        }
        if (count_2 != 0)
        {
            vec_2 = Helper.Div(vec_2,count_2);
            vec_2 = Helper.Sub(vec_2,boid.vel);
        }

        vec_1 = Helper.Mul(vec_1,power1);
        vec_2 = Helper.Mul(vec_2,power2);
        vec_3 = Helper.Mul(vec_3,power3);

        // if (debug)
        // {
        //     var msg = $"{Helper.ToHex(vec_2)}";
        //     System.Console.WriteLine(msg);
        // }

        Vector2 result = boid.vel;
        result = Helper.Add(Helper.Add(Helper.Add(result,vec_1),vec_2),vec_3);
        result = Vector2Ext.ClampLength(result,minSpeed,maxSpeed);
        boid.vel = result;
        return boid;
    }
    public static void UpdatePosition(ref Boid boid, ref AABB aabb, float deltaTime)
    {
        Vector2 velocityDelta = Helper.Mul(boid.vel,deltaTime);
        boid.pos = Helper.Add(boid.pos,velocityDelta);
        boid.pos = aabb.WrapAround(boid.pos);
    }
    public static void Print(ref Boid boid)
    {
        Helper.PrintVector2Hex(boid.pos);
        Helper.PrintVector2Hex(boid.vel);
        System.Console.WriteLine();
    }
}