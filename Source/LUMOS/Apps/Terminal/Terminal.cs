using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using LUMOS.Apps.Launcher;
using LUMOS.Liv;
using LUMOS.Misc;
using LUMOS.UICat.Core;
using System;
using System.Collections.Generic;

namespace LUMOS.Apps.Camera
{
    public class Terminal : LauncherApp
    {
        private const String _uuid = "opensystemquery.terminal";
        private const String _name = "Terminal";
        private const String _icon = "Assets/app-terminal.png";
        private const String _clearWin = "cls";
        private const String _clearLin = "clear";
        private const String _lumosExit = "LUMOS_EXIT";

        private readonly IDisplayManager _displayManager;

        private Double _windowWidth;
        private Double _windowHeight;

        private DockPanel? _terminalContent;

        public Terminal(IDisplayManager displayManager) : base(_icon, _name, _uuid)
        {
            _displayManager = displayManager;
        }

        public override void Start()
        {
            DockPanel terminalPanel = new()
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                Margin = new Thickness(10, 0, 10, 0),
            };

            TextBox commandBox = new()
            {
                Text = "help",
            };
            terminalPanel.Children.Add(commandBox);
            DockPanel.SetDock(commandBox, Dock.Top);

            TextBlock outputBlock = new()
            {
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                Foreground = new SolidColorBrush(Colors.White),
                FontFamily = new FontFamily("monospace,Consolas,DejaVu Sans Mono,SF Mono,Roboto Mono")
            };

            ScrollViewer textViewer = new()
            {
                Content = outputBlock,
                VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Visible,
                HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Disabled,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
            };

            terminalPanel.Children.Add(textViewer);
            DockPanel.SetDock(textViewer, Dock.Bottom);

            commandBox.KeyDown += (s, e) =>
            {
                if (e.Key == Avalonia.Input.Key.Enter)
                {
                    String command = commandBox.Text;
                    commandBox.Text = String.Empty;

                    if (command.ToLower() == _clearWin || command.ToLower() == _clearLin)
                    {
                        outputBlock.Text = String.Empty;
                        return;
                    }
                    if (command == _lumosExit) Environment.Exit(0);

                    CommandInvoker.ExecuteCommand(command,
                        (s) =>
                        {
                            Dispatcher.UIThread.Post(() =>
                            {
                                outputBlock.Text += $"\n{s}";
                                textViewer.ScrollToEnd();
                            });
                            _terminalContent = terminalPanel;

                        },
                        (s) =>
                        {
                            Dispatcher.UIThread.Post(() =>
                            {
                                outputBlock.Text += $"\n{s}";
                                textViewer.ScrollToEnd();
                            });
                            _terminalContent = terminalPanel;
                        });
                }
                _terminalContent = terminalPanel;
            };

            _terminalContent = terminalPanel;
        }

        public override void Stop()
        {
            _terminalContent = null;
        }

        public override List<CatApp> GetApp()
        {
            if (_terminalContent == null)
                Start();

            List<CatApp> contents = new();
            contents.Add(new(_terminalContent, CatDisplayMode.Primary));
            return contents;
        }

        public override void UpdateLayout(Double windowWidth, Double windowHeight)
        {
            _windowWidth = windowWidth;
            _windowHeight = windowHeight;
        }

        public override void OnPrevious()
        {
        }
    }
}
