using System.Diagnostics;

var path0 = AppDomain.CurrentDomain.BaseDirectory+@"..\..\..\";
var path1 = path0+@"..\Boids_wpf_3";
var path2 = path1+@"\bin\Release\net6.0-windows\Boids_wpf_3.exe";

ExecuteCommand($"dotnet build {path1} --configuration Release");

var processStartInfo = new ProcessStartInfo();
processStartInfo.FileName = path2;
processStartInfo.Arguments = Process.GetCurrentProcess().Id.ToString();
processStartInfo.RedirectStandardOutput = true;

var process = Process.Start(processStartInfo)!;
process.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
process.BeginOutputReadLine();
process.WaitForExit();

void ExecuteCommand(string command)
{
    var processStartInfo = new ProcessStartInfo();
    processStartInfo.FileName = "cmd.exe";
    processStartInfo.Arguments = "/c " + command;
    processStartInfo.CreateNoWindow = false;
    processStartInfo.UseShellExecute = false;
    processStartInfo.RedirectStandardError = true;
    processStartInfo.RedirectStandardOutput = true;

    var process = Process.Start(processStartInfo)!;
    process.WaitForExit();

    // Warning: This approach can lead to deadlocks, see Edit #2
    var output = process.StandardOutput.ReadToEnd();
    var error = process.StandardError.ReadToEnd();

    var exitCode = process.ExitCode;

    if (!string.IsNullOrEmpty(output)) Console.WriteLine(output);
    if (!string.IsNullOrEmpty(error)) Console.WriteLine(error);
    // Console.WriteLine("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
    // Console.WriteLine("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
    Console.WriteLine("ExitCode: " + exitCode.ToString(), "ExecuteCommand");
    Console.WriteLine();
    process.Close();
}