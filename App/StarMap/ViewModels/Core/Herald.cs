using Prism.Services;
using System;
using System.Threading.Tasks;

namespace StarMap.ViewModels.Core
{
  public abstract class Herald : Overseer
  {
    IPageDialogService _pageDialogService;

    public Herald(IPageDialogService pageDialogService)
    {
      _pageDialogService = pageDialogService;
    }
    
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
