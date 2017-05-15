using StarMap.Controls;
using System;
using Xamarin.Forms;

namespace StarMap.Views
{
  public partial class MainPage : ContentPage
  {
    public Overlay RightPanel { get; private set; }

    public Overlay BottomPanel { get; private set; }

    public MainPage()
    {      
      InitializeComponent();
    }

    protected override void OnAppearing()
    {
      RightPanel = new Overlay(rightOverlay, DockSide.Right, rightPanelButton.Width + rightOverlay.Padding.Left * 2);
      BottomPanel = new Overlay(bottomOverlay, DockSide.Bottom, bottomOverlay.RowDefinitions[0].Height.Value);

      RightPanel.Collapse(length: 0);
      BottomPanel.Collapse(length: 0);
      
      base.OnAppearing();
    }

    void OnButtonClicked(object sender, EventArgs args)
    {
      if (RightPanel.IsExpanded && constellationFilters.IsVisible)
        RightPanel.Collapse(Easing.CubicOut);
      else
        RightPanel.Expand(Easing.CubicIn);

      starFilters.IsVisible = false;
      constellationFilters.IsVisible = true;      
    }

    void OnStarFilterPanelButtonClicked(object sender, EventArgs args)
    {
      if (RightPanel.IsExpanded && starFilters.IsVisible)
        RightPanel.Collapse(Easing.CubicOut);
      else
        RightPanel.Expand(Easing.CubicIn);

      constellationFilters.IsVisible = false;
      starFilters.IsVisible = true;
    }

    void ResetFiltersButtonClicked(object sender, EventArgs args)
    {
      constellationFilters.SelectedItem = null;
    }

    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
      BottomPanel.Slide(Easing.CubicInOut);
    }
  }
}
