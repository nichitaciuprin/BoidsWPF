public static class Subgen
{
    public static int MAX = 1000000000;
    private static int[] state = new int[55];
    private static int pos;

    static Subgen()
    {
        Subgen.Init(0);
    }
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
        return Next() / (float)MAX;
    }
    public static float FractionSigned()
    {
        return FractionUnsigned() * 2 - 1;
    }
    public static float Range(float min, float max)
    {
        return min + ((max - min) * FractionUnsigned());
    }
}