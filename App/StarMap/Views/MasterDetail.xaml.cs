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

    protected override void OnAppearing()
    {
      base.OnAppearing();
      IsPresented = true;
    }
  }
}