using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Navigation;
using Prism.Services;

namespace StarMap.ViewModels.Core
{
  public abstract class Interlocutor : Navigator
  {
    IPageDialogService _pageDialogService;

    public Interlocutor(INavigationService navigationService, IPageDialogService pageDialogService)
      : base(navigationService)
    {
      _pageDialogService = pageDialogService;
    }

    // TODO: CONSTANTS!!!!
    protected async Task DisplayAlertAsync(string title, string message, string closeButton = "OK")
    {
      await _pageDialogService.DisplayAlertAsync(title, message, closeButton);
    }

    protected async Task<bool> DisplayDialogAsync(string title, string message, string acceptButton = "OK", string cancelButton = "Cancel")
    {
      return await _pageDialogService.DisplayAlertAsync(title, message, acceptButton, cancelButton);
    }
  }
}
