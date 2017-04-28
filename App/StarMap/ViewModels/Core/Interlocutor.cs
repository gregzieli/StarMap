using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Navigation;
using Prism.Services;

namespace StarMap.ViewModels.Core
{
  public abstract class Interlocutor : Overseer
  {
    IPageDialogService _pageDialogService;

    public Interlocutor(IPageDialogService pageDialogService)
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

    // The basic one, can be overriden if a more detailed action is needed.
    protected override async Task HandleException(Exception ex)
    {
      await DisplayAlertAsync("Error", ex.Message);
    }
  }
}
