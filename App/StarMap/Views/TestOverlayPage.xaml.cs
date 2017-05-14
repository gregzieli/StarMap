using StarMap.Controls;
using System;
using Xamarin.Forms;

namespace StarMap.Views
{
  public partial class TestOverlayPage : ContentPage
  {
    public Overlay RightPanel { get; private set; }

    public Overlay BottomPanel { get; private set; }

    public TestOverlayPage()
    {      
      InitializeComponent();
    }

    protected override void OnAppearing()
    {
      RightPanel = new Overlay(rightOverlay, DockSide.Right, rightPanelButton.Width + rightOverlay.Padding.Left * 2);
      BottomPanel = new Overlay(bottomOverlay, DockSide.Bottom, bottomOverlay.RowDefinitions[0].Height.Value);

      RightPanel.Collapse();
      BottomPanel.Collapse();

      base.OnAppearing();
    }

    void OnButtonClicked(object sender, EventArgs args)
    {
      RightPanel.Slide(Easing.CubicInOut);

      // This logic is strictly UI - No point in keeping it in the VM.
      if (constellations.SelectedItem != null)
        constellations.SelectedItem = null;
    }

    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
      BottomPanel.Slide(Easing.CubicInOut);
    }
  }
}
