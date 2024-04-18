using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using DynamicData;
using LUMOS.Liv;
using LUMOS.UICat.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LUMOS.Apps.Launcher
{
    public class Starport : AppBase
    {
        private const Int32 _launcherAppImageWidth = 128;
        private const Int32 _launcherLivImageWidth = 64;
        private const Int32 _margin = 10;
        private const Int32 _labelHeight = 25;
        private const String _uuid = "opensystemquery.starport";

        private readonly List<LauncherApp> _launcherApps;
        private readonly AppManager _appManager;
        private Control? _launcherControl;

        private Double _windowWidth;
        private Double _windowHeight;

        private Boolean _openLiv = false;

        public Starport(List<LauncherApp> launcherApps, AppManager appManager) : base(_uuid)
        {
            _launcherApps = launcherApps;
            _appManager = appManager;

            _appManager.Register(this as AppBase);
            _appManager.SwitchApp(Uuid);

            launcherApps.ForEach(app => _appManager.Register(app));
        }

        public override List<CatApp> GetApp()
        {
            if (_openLiv)
            {
                _openLiv = false;
                _launcherControl = GetLauncherLivGrid(_windowWidth, _windowHeight);
            }
            else
                _launcherControl = GetLauncherAppsGrid(_windowWidth, _windowHeight);
            CatApp catApp = new(_launcherControl, CatDisplayMode.Primary);
            List<CatApp> apps = new();
            apps.Add(catApp);
            return apps;
        }

        private Control GetLauncherLivGrid(Double windowWidth, Double windowHeight)
        {
            List<AppBase> activeApps = _appManager.GetActiveApps();
            List<LauncherApp> launcherApps = new();
            foreach (LauncherApp app in _launcherApps)
                foreach (AppBase activeApp in activeApps)
                    if (app.Uuid == activeApp.Uuid)
                        launcherApps.Add(app);

            Int32 columnCount = (Int32)(windowWidth / (_launcherLivImageWidth + 2 * _margin + _labelHeight));
            if (columnCount < 1) columnCount = 1;

            Grid grid = new()
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            };

            grid.ColumnDefinitions.Clear();
            for (Int32 i = 0; i < columnCount; i++)
            {
                ColumnDefinition column = new();
                column.Width = new GridLength(_launcherLivImageWidth + 2 * _margin + _labelHeight);
                grid.ColumnDefinitions.Add(column);
            }

            Int32 itemCount = launcherApps.Count();
            Int32 rowCount = (itemCount + columnCount - 1) / columnCount;
            grid.RowDefinitions.Clear();
            for (Int32 i = 0; i < rowCount; i++)
            {
                RowDefinition row = new();
                row.Height = new GridLength(_launcherLivImageWidth + 2 * _margin + _labelHeight);
                grid.RowDefinitions.Add(row);
            }

            for (Int32 i = 0, cell = 0; i < launcherApps.Count(); i++, cell++)
            {
                Int32 row = cell / columnCount;
                Int32 col = cell % columnCount;

                StackPanel appPanel = new()
                {
                    Orientation = Avalonia.Layout.Orientation.Vertical,
                    Margin = new Thickness(_margin),
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top
                };

                LauncherApp eventApp = launcherApps[i];
                appPanel.Tapped += (sender, e) =>
                {
                    _appManager.SwitchApp(Uuid);
                    _appManager.SwitchApp(eventApp.Uuid);
                };

                Label label = new()
                {
                    Content = launcherApps[i].Name,
                    Foreground = new SolidColorBrush(Colors.White),
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
                };

                String? assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
                Uri uri = new($"avares://{assemblyName}/{launcherApps[i].Icon}");

                Bitmap bitmap = new(AssetLoader.Open(uri));

                Image image = new()
                {
                    Source = bitmap,
                    Stretch = Stretch.UniformToFill,
                    Width = _launcherLivImageWidth,
                    Height = _launcherLivImageWidth,
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
                };

                appPanel.Children.Add(image);
                appPanel.Children.Add(label);

                Grid.SetRow(appPanel, row);
                Grid.SetColumn(appPanel, col);
                grid.Children.Add(appPanel);
            }

            ScrollViewer scrollViewer = new();
            scrollViewer.Content = grid;

            Label title = new()
            {
                Foreground = new SolidColorBrush(Colors.White),
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = "Recent",
                FontSize = 28
            };

            DockPanel dockPanel = new()
            {
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch
            };
            dockPanel.Children.Add(title);
            DockPanel.SetDock(title, Dock.Top);

            Button clearButton = new()
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                FontSize = 18,
                Foreground = new SolidColorBrush(Colors.White),
                Margin = new Thickness(10, 10, 10, 10),
                Content = "Clear"
            };
            dockPanel.Children.Add(clearButton);
            DockPanel.SetDock(clearButton, Dock.Bottom);

            List<AppBase> temporaryList = new() { activeApps };
            clearButton.Tapped += (s, e) =>
            {
                foreach (AppBase app in temporaryList) _appManager.TerminateApp(app.Uuid);
                _appManager.SwitchApp(Uuid);
            };

            dockPanel.Children.Add(scrollViewer);

            return dockPanel;

        }

        private Control GetLauncherAppsGrid(Double windowWidth, Double windowHeight)
        {
            Int32 columnCount = (Int32)(windowWidth / (_launcherAppImageWidth + 2 * _margin + _labelHeight));
            if (columnCount < 1) columnCount = 1;

            Grid grid = new()
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            };

            grid.ColumnDefinitions.Clear();
            for (Int32 i = 0; i < columnCount; i++)
            {
                ColumnDefinition column = new();
                column.Width = new GridLength(_launcherAppImageWidth + 2 * _margin + _labelHeight);
                grid.ColumnDefinitions.Add(column);
            }

            Int32 itemCount = _launcherApps.Count();
            Int32 rowCount = (itemCount + columnCount - 1) / columnCount;
            grid.RowDefinitions.Clear();
            for (Int32 i = 0; i < rowCount; i++)
            {
                RowDefinition row = new();
                row.Height = new GridLength(_launcherAppImageWidth + 2 * _margin + _labelHeight);
                grid.RowDefinitions.Add(row);
            }

            for (Int32 i = 0, cell = 0; i < _launcherApps.Count(); i++, cell++)
            {
                Int32 row = cell / columnCount;
                Int32 col = cell % columnCount;

                StackPanel appPanel = new()
                {
                    Orientation = Avalonia.Layout.Orientation.Vertical,
                    Margin = new Thickness(_margin),
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top
                };

                LauncherApp eventApp = _launcherApps[i];
                appPanel.Tapped += (sender, e) => _appManager.SwitchApp(eventApp.Uuid);

                Label label = new()
                {
                    Content = _launcherApps[i].Name,
                    Foreground = new SolidColorBrush(Colors.White),
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
                };

                String? assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
                Uri uri = new($"avares://{assemblyName}/{_launcherApps[i].Icon}");

                Bitmap bitmap = new(AssetLoader.Open(uri));

                Image image = new()
                {
                    Source = bitmap,
                    Stretch = Stretch.UniformToFill,
                    Width = _launcherAppImageWidth,
                    Height = _launcherAppImageWidth,
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
                };

                appPanel.Children.Add(image);
                appPanel.Children.Add(label);

                Grid.SetRow(appPanel, row);
                Grid.SetColumn(appPanel, col);
                grid.Children.Add(appPanel);
            }

            ScrollViewer scrollViewer = new();
            scrollViewer.Content = grid;

            Label title = new()
            {
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 28,
                Content = "Recent"
            };

            return scrollViewer;
        }

        public override void UpdateLayout(Double windowWidth, Double windowHeight)
        {
            _windowWidth = windowWidth;
            _windowHeight = windowHeight;

            if (_appManager.IsApp(Uuid))
                _appManager.Display(GetApp().First());
        }

        public override void OnPrevious()
        {
            _appManager.SwitchApp(Uuid);
        }

        public void OnLiv()
        {
            _openLiv = true;
        }

        public override void Start()
        {
        }

        public override void Stop()
        {
        }
    }
}
