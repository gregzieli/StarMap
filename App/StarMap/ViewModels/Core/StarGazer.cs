using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Navigation;
using Prism.Services;
using StarMap.Cll.Abstractions;
using Prism.Events;

namespace StarMap.ViewModels.Core
{
  // Not sure about this name now. Let's just assume, that this is just to take some load off the MainPageVM, not to store all the code in one place; 
  // The ethods here are more 'GENERAL', but I don't think any other VM would inherit this class.
  public class StarGazer : Navigator
  {
    public IStarManager StarManager { get; private set; }

    public StarGazer(INavigationService navigationService, IPageDialogService pageDialogService, IStarManager starManager) 
      : base(navigationService, pageDialogService)
    {
      StarManager = starManager;
    }

    //Probably that's too much. Just make the manager a property.
    // Should this Get() call Call(), or Call() should call Get()?
    protected T Get<T>(Func<IStarManager, T> fn)
    {
      return fn(StarManager);
    }
  }
}
