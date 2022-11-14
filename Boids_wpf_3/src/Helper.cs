using System.Diagnostics;

public static class Helper
{
    public static string ToHex(float value) => System.BitConverter.SingleToUInt32Bits(value).ToString("X");
    private static Stopwatch test = new Stopwatch();
    private static long allTime = 0;
    private static int count = 0;
    public static void Start()
    {
        test.Restart();
    }
    public static void End()
    {
        test.Stop();
        var deltaTime = test.ElapsedTicks;
        allTime += deltaTime;
        count++;
        if (count == 100)
        {
            // Loging.WriteLine($"AVG: {allTime/count}");
            allTime = 0;
            count = 0;
        }
    }
    public static long ToTicks(long milliseconds) => milliseconds*Stopwatch.Frequency/1000;
    public static float ToSecondsFloat(long ticks) => ((float)ToMilliseconds(ticks))/1000;
    public static long ToMilliseconds(long ticks) => ticks*1000/Stopwatch.Frequency;
    public static float Seconds(this long ticks)
    {
        // NEED TO RESERCH IT. https://pisquare.osisoft.com/s/Blog-Detail/a8r1I000000GvinQAC/how-can-something-so-simple-be-so-wrong-a-deep-dive-into-converting-ticks-to
        long wholeSecondPortion = (ticks / TimeSpan.TicksPerSecond) * TimeSpan.TicksPerSecond;
        long subsecondPortion = ticks - wholeSecondPortion;
        double wholeSeconds = wholeSecondPortion / (double)TimeSpan.TicksPerSecond;
        double subseconds = subsecondPortion / (double)TimeSpan.TicksPerSecond;
        var result = wholeSeconds + subseconds;
        return (float)result;
    }
    public static float RandFloat(Random rand, float min, float max)
    {
        System.Diagnostics.Debug.Assert(min < max);
        var length = max - min;
        var t = (rand.NextSingle()-.5f)*2f;
        return min + length*t;
    }
}