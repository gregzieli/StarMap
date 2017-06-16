using StarMap.Cll.Abstractions.Urho;
using StarMap.Controls;
using System;
using System.Diagnostics;
using Urho.Forms;
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

    protected override async void OnAppearing()
    {
      var urhoTask = ((IUrhoHandler)BindingContext).GenerateUrho(surface);

      RightPanel = new Overlay(rightOverlay, DockSide.Right, rightPanelButtons.Width + rightOverlay.Padding.Left * 2);
      BottomPanel = new Overlay(bottomOverlay, DockSide.Bottom, bottomOverlay.RowDefinitions[0].Height.Value);

      RightPanel.Collapse(length: 0);
      BottomPanel.Collapse(length: 0);

      await urhoTask.ConfigureAwait(false);

      //base.OnAppearing();
    }

    protected override void OnDisappearing()
    {
      UrhoSurface.OnDestroy();
      base.OnDisappearing();
    }

    void OnConstellationsButtonClicked(object sender, EventArgs args)
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

    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
      BottomPanel.Slide(Easing.CubicInOut);
    }
  }
}
