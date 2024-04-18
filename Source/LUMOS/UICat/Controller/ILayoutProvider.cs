using Avalonia;
using Avalonia.Controls;

namespace LUMOS.UICat.Controller
{
    public interface ILayoutProvider
    {
        public Control GetControl();
        public Size GetSize();
        public void SetPrimaryContent(Control control);
        public void SetNotificationContent(Control control);
        public void SetNavigationContent(Control control);
        public void SetOverlayContent(Control control);
        public void SetFullscreenContent(Control control);
        public void SetKeyboardContent(Control control);
        public void ClearPrimaryContent();
        public void ClearNotificationContent();
        public void ClearNavigationContent();
        public void ClearOverlayContent();
        public void ClearFullscreenContent();
        public void ClearKeyboardContent();

    }
}
