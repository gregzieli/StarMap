using StarMap.Cll.Abstractions.Urho;
using StarMap.Cll.Exceptions;
using StarMap.Urho;
using StarMap.ViewModels;
using StarMap.ViewModels.Core;
using Urho.Forms;
using Xamarin.Forms;

namespace StarMap.Views
{
  public partial class StarDetailPage : ContentPage
  {
    public StarDetailPage()
    {
      InitializeComponent();
    }

    protected override async void OnAppearing()
    {
      await ((IUrhoHandler)BindingContext).GenerateUrho(surface);
    }

    protected override void OnDisappearing()
    {
      UrhoSurface.OnDestroy();
      base.OnDisappearing();
    }

    //protected override bool OnBackButtonPressed()
    //{
    //  var a = (ViewModels.Core.Navigator)BindingContext;
    //  a.NavigateTest(new Uri("ms-app:///MasterDetail/MainPage", UriKind.Absolute));
    //  return true;
    //}
  }
}
