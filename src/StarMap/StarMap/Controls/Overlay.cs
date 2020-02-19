using System;
using Xamarin.Forms;

namespace StarMap.Controls
{
  /// <summary>
  /// Helper class for the overlay control
  /// </summary>
  public class Overlay
  {
        private VisualElement _element;

    public Rectangle Bounds { get; private set; }

    public DockSide DockSide { get; private set; }

    public Vec2 Collapsed { get; private set; }

    public Vec2 Expanded { get; private set; }

    /// <summary>
    /// Width or height
    /// </summary>
    public double CollapsedSize { get; private set; }

    public bool IsExpanded { get; set; }

    public Overlay(VisualElement element, DockSide dockSide, double collapsedSize)
    {
      _element = element;
      Bounds = element.Bounds;
      DockSide = dockSide;
      CollapsedSize = collapsedSize;
      SetBounds();
    }

        private void SetBounds()
    {
      switch (DockSide)
      {
        case DockSide.Right:
          Expanded = new Vec2(0, Bounds.Y);
          Collapsed = new Vec2(Bounds.Width - CollapsedSize, Bounds.Y);
          break;
        case DockSide.Bottom:
          Expanded = new Vec2(Bounds.X, 0);
          Collapsed = new Vec2(Bounds.X, Bounds.Height - CollapsedSize);
          break;
        case DockSide.Left:
        case DockSide.Top:
          throw new NotImplementedException("Left and top are system sliders.");
      }
    }

    public void Expand(Easing easing = null, uint length = 250)
    {
      _element.TranslateTo(Expanded.X, Expanded.Y, length, easing);
      IsExpanded = true;
    }

    public void Collapse(Easing easing = null, uint length = 250)
    {
      _element.TranslateTo(Collapsed.X, Collapsed.Y, length, easing);
      IsExpanded = false;
    }

    public void Slide(Easing easing = null, uint length = 250)
    {
      if (IsExpanded)
        Collapse(easing, length);
      else
        Expand(easing, length);
    }
  }


  public enum DockSide
  {
    Left,

    Right,

    Top,

    Bottom
  }
}
