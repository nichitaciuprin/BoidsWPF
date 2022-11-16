using System.IO;
using System.Security;
using System.Runtime.InteropServices;

[SuppressUnmanagedCodeSecurity]
public static class ConsoleManager
{
    private const string Kernel32_DllName = "kernel32.dll";

    [DllImport(Kernel32_DllName)]
    private static extern bool AllocConsole();

    [DllImport(Kernel32_DllName)]
    private static extern bool FreeConsole();

    [DllImport(Kernel32_DllName)]
    private static extern IntPtr GetConsoleWindow();

    [DllImport(Kernel32_DllName)]
    private static extern int GetConsoleOutputCP();

    public static bool HasConsole => GetConsoleWindow() != IntPtr.Zero;

    /// <summary>
    /// Creates a new console instance if the process is not attached to a console already.
    /// </summary>
    public static void Show()
    {
        //#if DEBUG
        if (HasConsole) return;
        AllocConsole();
        InvalidateOutAndError();
        //#endif
    }
    /// <summary>
    /// If the process has a console attached to it, it will be detached and no longer visible. Writing to the System.Console is still possible, but no output will be shown.
    /// </summary>
    public static void Hide()
    {
        //#if DEBUG
        if (!HasConsole) return;
        Console.SetOut(TextWriter.Null);
        Console.SetError(TextWriter.Null);
        FreeConsole();
        //#endif
    }
    public static void Toggle()
    {
        if (HasConsole)
            Hide();
        else
            Show();
    }
    private static void InvalidateOutAndError()
    {
        // var type = typeof(System.Console);
        // var _out = type.GetField("_out",System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)!;
        // var _error = type.GetField("_error",System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)!;
        // var _InitializeStdOutError = type.GetMethod("InitializeStdOutError",System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)!;

        // Debug.Assert(_out != null);
        // Debug.Assert(_error != null);
        // Debug.Assert(_InitializeStdOutError != null);

        // _out.SetValue(null, null);
        // _error.SetValue(null, null);
        // _InitializeStdOutError.Invoke(null, new object[] { true });
    }
}