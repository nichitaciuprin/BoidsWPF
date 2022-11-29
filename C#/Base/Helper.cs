using System.Diagnostics;

public static class Helper
{
    public static void PrintFloatHex(float f)
    {
        var msg = System.BitConverter.SingleToUInt32Bits(f).ToString("X");
        Console.Write(msg);
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
}