using Avalonia;
using Avalonia.Controls;
using LUMOS.UICat.Controller;
using LUMOS.UICat.Core;
using System;

namespace LUMOS.UICat.Views;

public partial class CoreView : UserControl, ILayoutProvider, IInputEssentials, ICatBridge
{
    private readonly CatalystEngine _catalystEngine;
    private Size _arrangedSize;
    private readonly Lumos _lumos;

    public CoreView()
    {
        InitializeComponent();
        this._catalystEngine = new(this as ILayoutProvider, this as IInputEssentials);
        this._lumos = new Lumos(this as ICatBridge);
    }

    public void EvaluateSize(Double windowWidth, Double windowHeight)
    {
        _lumos.UpdateLayout(windowWidth, windowHeight);
    }

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        EvaluateSize(this._arrangedSize.Width, this._arrangedSize.Height);
        base.OnSizeChanged(e);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        this._arrangedSize = finalSize;
        return base.ArrangeOverride(finalSize);
    }

    public Control GetControl()
    {
        return this.LumosContent;
    }

    public Size GetSize()
    {
        return this._arrangedSize;
    }

    public void SetPrimaryContent(Control control)
    {
        this.LumosContent.Children.Clear();
        this.LumosContent.Children.Add(control);
    }

    public void SetNotificationContent(Control control)
    {
        this.LumosNotifications.Children.Clear();
        this.LumosNotifications.Children.Add(control);
    }

    public void SetNavigationContent(Control control)
    {
        this.LumosNavigation.Children.Clear();
        this.LumosNavigation.Children.Add(control);
    }

    public void SetOverlayContent(Control control)
    {
        this.LumosOverlay.Children.Clear();
        this.LumosOverlay.Children.Add(control);
        this.LumosOverlay.IsVisible = true;
    }

    public void SetFullscreenContent(Control control)
    {
        SetSoftKeysVisible(false);
        this.LumosFullscreen.Children.Clear();
        this.LumosFullscreen.Children.Add(control);
        this.LumosFullscreen.IsVisible = true;
        control.Width = this._arrangedSize.Width;
        control.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch;
        this.LumosNotifications.IsVisible = false;
    }

    public void SetKeyboardContent(Control keyboard)
    {
        this.LumosKeyboard.Children.Clear();
        this.LumosKeyboard.Children.Add(keyboard);
    }

    public void ClearPrimaryContent()
    {
        this.LumosContent.Children.Clear();
    }

    public void ClearNotificationContent()
    {
        this.LumosNotifications.Children.Clear();
    }

    public void ClearNavigationContent()
    {
        this.LumosNavigation.Children.Clear();
    }

    public void ClearOverlayContent()
    {
        this.LumosOverlay.Children.Clear();
        this.LumosOverlay.IsVisible = false;
    }

    public void ClearFullscreenContent()
    {
        SetSoftKeysVisible(true);
        this.LumosFullscreen.Children.Clear();
        this.LumosFullscreen.IsVisible = false;
        this.LumosNotifications.IsVisible = true;
    }

    public void ClearKeyboardContent()
    {
        this.LumosKeyboard.Children.Clear();
    }

    public void SetSoftKeysVisible(Boolean visible)
    {
        this.LumosNavigation.IsVisible = visible;
    }

    public void FocussedSearchBox(TextBox textBox)
    {
        _lumos.FocussedSearchBox(textBox);
    }

    public void Display(CatApp catApp)
    {
        this._catalystEngine.SetContent(catApp);
    }
}
