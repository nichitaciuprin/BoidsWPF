public class Subgen
{
    private static int MAX = 1000000000;
    private int[] state = new int[55];
    private int pos = 54;

    public Subgen(int seed)
    {
        int[] temp = new int[55];
        temp[0] = Mod(seed);
        temp[1] = 1;

        for(int i = 2; i < 55; ++i) temp[i] = Mod(temp[i - 2] - temp[i - 1]);
        for(int i = 0; i < 55; ++i) state[i] = temp[(34 * (i + 1)) % 55];
        for(int i = 55; i < 220; ++i) Next();
    }
    private static int Mod(int n)
    {
        return ((n % MAX) + MAX) % MAX;
    }
    public int Next()
    {
        int temp = Mod(state[(pos + 1) % 55] - state[(pos + 32) % 55]);
        pos = (pos + 1) % 55;
        state[pos] = temp;
        return temp;
    }
    public float FractionUnsigned()
    {
        return Next() / (float)MAX;
    }
    public float FractionSigned()
    {
        return FractionUnsigned() * 2 - 1;
    }
    public float Range(float min, float max)
    {
        return min + ((max - min) * FractionUnsigned());
    }
}