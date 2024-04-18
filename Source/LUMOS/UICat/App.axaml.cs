using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using LUMOS.UICat.Views;
using System.Linq;

namespace LUMOS.UICat;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new WrapWindow
            {
            };
            if (desktop.Args != null && desktop.Args.Contains(Arguments.ArgumentFullScreenLock))
            {
                desktop.MainWindow.WindowState = Avalonia.Controls.WindowState.FullScreen;
                desktop.MainWindow.SystemDecorations = Avalonia.Controls.SystemDecorations.None;
                desktop.MainWindow.Closing += (s, e) => { e.Cancel = true; };
            }
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new CoreView
            {
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
