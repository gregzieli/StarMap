using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace StarMap.Controls
{
  public partial class StatusPanel : Grid
  {
    public StatusPanel()
    {
      InitializeComponent();
    }

    protected override void OnAdded(View view)
    {
      base.OnAdded(view);
      earthButton.IsVisible = false;
      // Why bother with extra properties to bind to
      earthButton.Clicked += (s, e) => earthButton.IsVisible = false;
      travelButton.Clicked += (s, e) => earthButton.IsVisible = true;
    }
  }
}