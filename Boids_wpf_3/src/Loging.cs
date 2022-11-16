using System.IO;
using System.Text;

public static class Loging
{
    private static int size = 10000;
    private static StringBuilder buffer = new StringBuilder(size,size);
    private static readonly string projectRootPath = AppDomain.CurrentDomain.BaseDirectory+@"..\..\..\";
    private static readonly string logFilePath = projectRootPath+"logs.txt";
    public static void CreateOrClearFile()
    {
        if (File.Exists(logFilePath))
            File.Create(logFilePath);
        else
            File.WriteAllText(logFilePath,string.Empty);
    }
    public static void WriteToFile()
    {
        using var streamWriter = File.CreateText(logFilePath);
        streamWriter.WriteLine(buffer);
    }
    public static void WriteLine(string str)
    {
        lock (buffer)
        {
            try
            {
                buffer.AppendLine(str);
            }
            catch (System.Exception)
            {
            }
        }
    }
}