using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StarMap.Views
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class WebViewPage : ContentPage
  {
    public WebViewPage()
    {
      InitializeComponent();
    }

    private void backClicked(object sender, EventArgs e)
    {
      if (Browser.CanGoBack)
        Browser.GoBack();
    }

    private void forwardClicked(object sender, EventArgs e)
    {
      if (Browser.CanGoForward)
        Browser.GoForward();
    }

    void webOnNavigating(object sender, WebNavigatingEventArgs e)
      => preloader.IsRunning = true;

    void webOnEndNavigating(object sender, WebNavigatedEventArgs e)
      => preloader.IsRunning = false;
  }
}