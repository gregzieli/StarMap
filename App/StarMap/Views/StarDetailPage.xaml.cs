using StarMap.Cll.Abstractions.Urho;
using Urho.Forms;
using Xamarin.Forms;
using urho = Urho;

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
      base.OnAppearing();
      await ((IUrhoHandler)BindingContext).GenerateUrho(surface);
    }
    
    protected override async void OnDisappearing()
    {
      base.OnDisappearing();
      await urho.Application.Current.Exit();
      UrhoSurface.OnDestroy();
    }

    //protected override bool OnBackButtonPressed()
    //{
    //  var a = (ViewModels.Core.Navigator)BindingContext;
    //  a.NavigateTest(new Uri("ms-app:///MasterDetail/MainPage", UriKind.Absolute));
    //  return true;
    //}
  }
}
