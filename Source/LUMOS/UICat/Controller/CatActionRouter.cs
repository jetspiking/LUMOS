using Avalonia.Controls;
using Avalonia.VisualTree;
using System.Linq;

namespace LUMOS.UICat.Controller
{
    public static class CatActionRouter
    {
        public static void SetCallbacks(Control control, IInputEssentials callback)
        {
            AttachHandlersToControl(control, callback);
        }

        private static void AttachHandlersToControl(Control control, IInputEssentials callback)
        {
            AttachEventHandlers(control, callback);

            foreach (Control child in control.GetVisualChildren().OfType<Control>())
                AttachHandlersToControl(child, callback);
        }

        private static void AttachEventHandlers(Control control, IInputEssentials callback)
        {
            if (control is TextBox textBox)
            {
                textBox.AddHandler(TextBox.PointerReleasedEvent, (s, e) => callback.FocussedSearchBox(textBox), Avalonia.Interactivity.RoutingStrategies.Bubble);
                textBox.AddHandler(TextBox.TappedEvent, (s, e) => callback.FocussedSearchBox(textBox), Avalonia.Interactivity.RoutingStrategies.Bubble);
            }

            //if (control is TextBlock textBlock)
        }
    }
}
