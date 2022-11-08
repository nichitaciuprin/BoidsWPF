using System.Numerics;

public struct Boid
{
    public Vector2 position;
    public Vector2 velocity;
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
        this.position = aabb.RandPointInside;
        this.velocity = Helper.Mul(randDirection,randSpeed);
    }
    public static void UpdateVelocity(int thisBoidIndex, Boid[] boids, bool debug)
    {
        Vector2 vec_1 = Vector2.Zero;
        Vector2 vec_2 = Vector2.Zero;
        Vector2 vec_3 = Vector2.Zero;
        int count_1 = 0;
        int count_2 = 0;

        // if (debug)
        // {
        //     Helper.PrintVector2Hex(boids[thisBoidIndex].velocity);
        //     System.Console.WriteLine();
        // }

        var boid = boids[thisBoidIndex];
        for (int i = 0; i < boids.Length; i++)
        {
            if (thisBoidIndex == i) continue;
            var otherBoid = boids[i];

            var diff = Helper.Sub(boid.position,otherBoid.position);
            var dist = Helper.Length(diff);

            // if (debug)
            // {
            //     Helper.PrintVector2Hex(otherBoid.position);
            //     System.Console.WriteLine();
            // }

            // COHESION
            if (dist < range_1)
            {
                vec_1 = Helper.Add(vec_1,otherBoid.position);
                count_1++;
            }

            // ALIGHMENT
            if (dist < range_2)
            {
                vec_2 = Helper.Add(vec_2,otherBoid.velocity);
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

        if (count_1 != 0)
        {
            vec_1 = Helper.Div(vec_1,count_1);
            vec_1 = Helper.Sub(vec_1,boid.position);
        }
        if (count_2 != 0)
        {
            vec_2 = Helper.Div(vec_2,count_2);
            vec_2 = Helper.Sub(vec_2,boid.velocity);
        }

        vec_1 = Helper.Mul(vec_1,power1);
        vec_2 = Helper.Mul(vec_2,power2);
        vec_3 = Helper.Mul(vec_3,power3);

        Vector2 result = boid.velocity;
        System.Console.Write(count_1);
        // Helper.PrintVector2Hex(vec_1);
        System.Console.WriteLine();
        // if (debug)
        // {
        //     // System.Console.Write(count_1);
        //     // Helper.PrintVector2Hex(vec_1);
        //     // System.Console.WriteLine();
        // }
        result = Helper.Add(Helper.Add(Helper.Add(result,vec_1),vec_2),vec_3);
        result = Vector2Ext.ClampLength(result,minSpeed,maxSpeed);
        boid.velocity = result;

        boids[thisBoidIndex] = boid;
    }
    public static void UpdatePosition(ref Boid boid, ref AABB aabb, float deltaTime)
    {
        Vector2 velocityDelta = Helper.Mul(boid.velocity,deltaTime);
        boid.position = Helper.Add(boid.position,velocityDelta);
        boid.position = aabb.WrapAround(boid.position);
    }
    public static void Print(ref Boid boid)
    {
        Helper.PrintVector2Hex(boid.position);
        Helper.PrintVector2Hex(boid.velocity);
        System.Console.WriteLine();
    }
}