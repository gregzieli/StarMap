using System;
using Xamarin.Forms;

namespace StarMap.Views
{
  public partial class MasterDetail : MasterDetailPage
  {
    public MasterDetail()
    {
      InitializeComponent();
    }
    
    void SwitchChanged(object sender, ToggledEventArgs e)
    {
    }

    // This logic is strictly UI - No point in keeping it in the VM.
    void HideConstellations(object sender, EventArgs e)
    {
      constellations.IsVisible = !constellations.IsVisible;
      if (constellations.SelectedItem != null)
        constellations.SelectedItem = null;
    }
  }
}