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

    public MainPage() => InitializeComponent();

    protected override async void OnAppearing()
    {
      base.OnAppearing();

      //earthButton.IsVisible = false;
      //// Why bother with extra properties to bind to
      //earthButton.Clicked += (s, e) => earthButton.IsVisible = false;
      //travelButton.Clicked += (s, e) => earthButton.IsVisible = true;

      RightPanel = new Overlay(rightOverlay, DockSide.Right, rightPanelButtons.Width + rightOverlay.Padding.Left * 2);

      RightPanel.Collapse(length: 0);

      await ((IUrhoHandler)BindingContext).GenerateUrho(surface);      
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
  }
}
