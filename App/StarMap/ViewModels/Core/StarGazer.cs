using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Navigation;
using Prism.Services;
using StarMap.Cll.Abstractions;

namespace StarMap.ViewModels.Core
{
  public class StarGazer : Navigator
  {
    public IStarManager StarManager { get; private set; }

    public StarGazer(INavigationService navigationService, IPageDialogService pageDialogService, IStarManager starManager) 
      : base(navigationService, pageDialogService)
    {
      StarManager = starManager;
    }

    //Probably that's too much. Just make the manager a property.
    protected T Get<T>(Func<IStarManager, T> fn)
    {
      return fn(StarManager);
    }
  }
}
