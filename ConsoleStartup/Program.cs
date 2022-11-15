using System.Diagnostics;

var path0 = AppDomain.CurrentDomain.BaseDirectory+@"..\..\..\";
var path1 = path0+@"..\Boids_wpf_3";
var path2 = path1+@"\bin\Release\net6.0-windows\Boids_wpf_3.exe";

ExecuteCommand($"dotnet build {path1} --configuration Release");

var startInfo = new ProcessStartInfo();
startInfo.RedirectStandardOutput = true;
startInfo.FileName = path2;
var process = Process.Start(startInfo)!;
// while(!process.HasExited)
// {
//     var result = process.StandardOutput.Read();
//     if (result != -1)
//         Console.WriteLine((char)result);
//     System.Threading.Thread.Yield();
// }
process.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
process.BeginOutputReadLine();
process.WaitForExit();

void ExecuteCommand(string command)
{
    int exitCode;
    ProcessStartInfo processInfo;
    Process process;

    processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
    processInfo.CreateNoWindow = false;
    processInfo.UseShellExecute = false;
    processInfo.RedirectStandardError = true;
    processInfo.RedirectStandardOutput = true;

    process = Process.Start(processInfo)!;
    process.WaitForExit();

    // Warning: This approach can lead to deadlocks, see Edit #2
    string output = process.StandardOutput.ReadToEnd();
    string error = process.StandardError.ReadToEnd();

    exitCode = process.ExitCode;

    if (!string.IsNullOrEmpty(output)) Console.WriteLine(output);
    if (!string.IsNullOrEmpty(error)) Console.WriteLine(error);
    // Console.WriteLine("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
    // Console.WriteLine("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
    Console.WriteLine("ExitCode: " + exitCode.ToString(), "ExecuteCommand");
    process.Close();
}