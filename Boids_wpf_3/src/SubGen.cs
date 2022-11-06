public static class Subgen
{
    public static int MAX = 1000000000;
    private static int[] state = new int[55];
    private static int pos;

    private static int mod(int n)
    {
        return ((n % MAX) + MAX) % MAX;
    }
    public static void Init(int seed)
    {
        int[] temp = new int[55];
        temp[0] = mod(seed);
        temp[1] = 1;
        for(int i = 2; i < 55; ++i)
            temp[i] = mod(temp[i - 2] - temp[i - 1]);

        for(int i = 0; i < 55; ++i)
            state[i] = temp[(34 * (i + 1)) % 55];

        pos = 54;
        for(int i = 55; i < 220; ++i)
            Next();
    }
    public static int Next()
    {
        int temp = mod(state[(pos + 1) % 55] - state[(pos + 32) % 55]);
        pos = (pos + 1) % 55;
        state[pos] = temp;
        return temp;
    }
    public static float FractionUnsigned()
    {
        var next = Next();
        // System.Console.WriteLine(next);
        return next / (float)MAX;
    }
    public static float FractionSigned()
    {
        float result = FractionUnsigned() * 2 - 1;
        // Helper.PrintFloatHex(result);
        // System.Console.WriteLine();
        return result;
    }
    public static float Range(float min, float max)
    {
        float result = min + ((max - min) * FractionUnsigned());
        // Helper.PrintFloatHex(result);
        // System.Console.WriteLine();
        return result;
    }
}

/*
0.824359 = 824358983 / 1000000000.000000
0.925189 = 925189384 / 1000000000.000000
0.988325 = 988325069 / 1000000000.000000
0.030883 = 30882955 / 1000000000.000000
0.367453 = 367453453 / 1000000000.000000

0.824359 = 824358983 / 1E+09
0.9251894 = 925189384 / 1E+09
0.98832506 = 988325069 / 1E+09
0.030882956 = 30882955 / 1E+09
0.36745343 = 367453453 / 1E+09
*/