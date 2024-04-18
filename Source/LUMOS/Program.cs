using Avalonia;
using Avalonia.ReactiveUI;
using LUMOS.UICat;
using Projektanker.Icons.Avalonia;
using Projektanker.Icons.Avalonia.FontAwesome;
using System;
using System.Linq;
using System.Threading;

namespace UICat.Desktop;

class Program
{
    [STAThread]
    public static int Main(String[] args)
    {
        AppBuilder builder = BuildAvaloniaApp();
        if (args.Contains(Arguments.ArgumentDirectRenderingManager))
        {
            SilenceConsole();

            return builder.StartLinuxDrm(args, null, 1D);
        }

        return builder.StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
    {
        IconProvider.Current.Register<FontAwesomeIconProvider>();
        return AppBuilder.Configure<App>()
               .UsePlatformDetect()
               .WithInterFont()
               .LogToTrace()
               .UseReactiveUI();
    }

    private static void SilenceConsole()
    {
        new Thread(() =>
            {
                Console.CursorVisible = false;
                while (true)
                    Console.ReadKey(true);
            })
        { IsBackground = true }.Start();
    }
}
