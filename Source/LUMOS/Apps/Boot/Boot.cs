using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using LUMOS.Apps.Launcher;
using LUMOS.Liv;
using LUMOS.UICat.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace LUMOS.Apps.Boot
{
    public class Boot : LauncherApp
    {
        private const String _uuid = "opensystemquery.boot";
        private const String _name = "Boot";
        private const String _icon = "Assets/app-boot.png";

        private Double _windowWidth;
        private Double _windowHeight;

        private readonly IDisplayManager _displayManager;

        public Boot(IDisplayManager displayManager) : base(_icon, _name, _uuid)
        {
            _displayManager = displayManager;
        }

        public override void Start()
        {
            _beginDisplay();
        }

        public override void Stop()
        {
        }

        public override List<CatApp> GetApp()
        {
            _beginDisplay();
            return new List<CatApp>();
        }

        private async void _beginDisplay()
        {
            for (Int32 i = 0; i < 20; i++)
            {
                _displayManager.Display(new CatApp(GetImageFrame(i), CatDisplayMode.Fullscreen));
                await Task.Delay(125);
            }
            _displayManager.Display(new CatApp(null, CatDisplayMode.ClearFullscreen));
        }

        private Image GetImageFrame(Int32 frame)
        {
            String? assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            Uri uri = new($"avares://{assemblyName}/Assets/app-boot{frame}.png");

            Bitmap bitmap = new(AssetLoader.Open(uri));

            Image image = new()
            {
                Source = bitmap,
                Stretch = Stretch.Uniform,
                Width = _windowWidth / 1.5,
                Height = _windowHeight / 1.5,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
            };

            return image;
        }

        public override void OnPrevious()
        {

        }

        public override void UpdateLayout(Double windowWidth, Double windowHeight)
        {
            _windowWidth = windowWidth;
            _windowHeight = windowHeight;
        }
    }
}
