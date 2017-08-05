using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using StarMap.Cll.Constants;
using StarMap.ViewModels.Core;
using System.Collections.Generic;

namespace StarMap.ViewModels
{
  public class AboutPageViewModel : Navigator
  {
    public AboutPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) 
      : base(navigationService, pageDialogService) { }

    private DelegateCommand<string> _webCommand;
    public DelegateCommand<string> WebCommand =>
        _webCommand ?? (_webCommand = new DelegateCommand<string>(Web));

    private async void Web(string url)
    {
      // Unfortunately, I couldn't use simply in xaml 
      // NavigateCommand with CommandParameter="{Binding Url, StringFormat='WebViewPage?url={0}'}",
      // Because prism treats that deepLinking, and throws that I didn't register the url :D:D
      await Navigate(Navigation.WebView, Navigation.Keys.Url, url);
    }

    IList<Source> _sources = new List<Source>
    {
      new Source("HYG Database", "http://www.astronexus.com/hyg"),
      new Source("Xamarin.Forms", "https://github.com/xamarin/Xamarin.Forms"),
      new Source("SQLite-net", "https://github.com/praeclarum/sqlite-net"),
      new Source("UrhoSharp", "https://github.com/xamarin/urho"),
      new Source("Prism", "https://github.com/PrismLibrary/Prism"),
      new Source("Xamarin Plugins", "https://github.com/xamarin/XamarinComponents"),
      new Source("Iconfinder", "https://www.iconfinder.com"),
      new Source("Star texture", "http://www.celestiamotherlode.net/catalog/extrasolar_stars.php"),
      new Source("Icon8", "http://ic8.link/33242")
    };
    public IList<Source> Sources => _sources;

    public class Source
    {
      public Source(string name, string url)
      {
        Name = name;
        Url = url;
      }
      public string Name { get; set; }

      public string Url { get; set; }
    }
  }  
}
