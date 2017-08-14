using System.Threading;
using Xamarin.Forms;

namespace StarMap.Views
{
  public partial class StartPage : ContentPage
  {
    CancellationTokenSource cts = new CancellationTokenSource();

    public StartPage()
    {
      InitializeComponent();
    }

    protected override async void OnAppearing()
    {      
      base.OnAppearing();
      while (!cts.IsCancellationRequested)
      {
        await navArrow.TranslateTo(100, navArrow.TranslationY, length: 1000, easing: Easing.CubicOut);
        await navArrow.TranslateTo(0, navArrow.TranslationY, length: 1000, easing: Easing.SinIn);
      }
    }

    protected override void OnDisappearing()
    {
      base.OnDisappearing();
      cts.Cancel();
    }
    
  }
}
