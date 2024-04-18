using LUMOS.UICat.Core;

namespace LUMOS.UICat.Controller
{
    public class CatalystEngine : ICatalyst
    {
        private readonly ILayoutProvider _layoutProvider;
        private readonly IInputEssentials _catCallback;

        public CatalystEngine(ILayoutProvider layoutProvider, IInputEssentials callCallback)
        {
            this._layoutProvider = layoutProvider;
            this._catCallback = callCallback;
        }

        public void SetContent(CatApp catApp)
        {
            switch (catApp.DisplayMode)
            {
                case CatDisplayMode.Primary:
                    if (catApp.Control != null)
                        this._layoutProvider.SetPrimaryContent(catApp.Control);
                    break;
                case CatDisplayMode.Overlay:
                    if (catApp.Control != null)
                        this._layoutProvider.SetOverlayContent(catApp.Control);
                    break;
                case CatDisplayMode.Notification:
                    if (catApp.Control != null)
                        this._layoutProvider.SetNotificationContent(catApp.Control);
                    break;
                case CatDisplayMode.Navigation:
                    if (catApp.Control != null)
                        this._layoutProvider.SetNavigationContent(catApp.Control);
                    break;
                case CatDisplayMode.Fullscreen:
                    if (catApp.Control != null)
                        this._layoutProvider.SetFullscreenContent(catApp.Control);
                    break;
                case CatDisplayMode.Keyboard:
                    if (catApp.Control != null)
                        this._layoutProvider.SetKeyboardContent(catApp.Control);
                    break;
                case CatDisplayMode.ClearPrimary:
                    this._layoutProvider.ClearPrimaryContent();
                    break;
                case CatDisplayMode.ClearOverlay:
                    this._layoutProvider.ClearOverlayContent();
                    break;
                case CatDisplayMode.ClearNotification:
                    this._layoutProvider.ClearNotificationContent();
                    break;
                case CatDisplayMode.ClearNavigation:
                    this._layoutProvider.ClearNavigationContent();
                    break;
                case CatDisplayMode.ClearFullscreen:
                    this._layoutProvider.ClearFullscreenContent();
                    break;
                case CatDisplayMode.ClearKeyboard:
                    this._layoutProvider.ClearKeyboardContent();
                    break;

            }

            if (catApp.Control != null)
                CatActionRouter.SetCallbacks(catApp.Control, _catCallback);
        }
    }
}
