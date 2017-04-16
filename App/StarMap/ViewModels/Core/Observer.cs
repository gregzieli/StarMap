using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarMap.ViewModels.Core
{
  public abstract class Observer : BaseViewModel
  {
    /// <summary>
    /// Ensures that no command is executed when a VM is busy.
    /// </summary>
    /// <returns>true, if not busy; false otherwise.</returns>
    protected virtual bool CanExecute() => !IsBusy;
  }
}
