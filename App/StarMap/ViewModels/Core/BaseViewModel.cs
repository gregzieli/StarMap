using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarMap.ViewModels.Core
{
  public abstract class BaseViewModel : BindableBase
  {
    private bool _isBusy = false;
    public bool IsBusy
    {
      get { return _isBusy; }
      set { SetProperty(ref _isBusy, value); }
    }

    /// <summary>
    /// Executes a delegate that returns a value of the given type, 
    /// catching any exception that may happen during execution.
    /// </summary>
    /// <typeparam name="A">Type returned by the delegate.</typeparam>
    /// <param name="fn">Delegate to execute.</param>
    /// <param name="onException">An action to execute upon catching an exception.</param>
    protected void Call(Action fn, Action<Exception> onException = null)
    {
      try
      {
        fn();
      }
      catch (Exception ex)
      {
        onException?.Invoke(ex);
      }
    }

    /// <summary>
    /// Executes a delegate that returns a value of the given type, 
    /// catching any exception that may happen during execution.
    /// </summary>
    /// <typeparam name="A">Type returned by the delegate.</typeparam>
    /// <param name="fn">Delegate to execute.</param>
    /// <param name="onException">An action to execute upon catching an exception.</param>
    protected A Call<A>(Func<A> fn, Action<Exception> onException = null)
    {
      try
      {
        return fn();
      }
      catch (Exception ex)
      {
        onException?.Invoke(ex);
        return default(A);
      }
    }

    /// <summary>
    /// Executes a delegate that returns a value of the given type asynchronously, 
    /// catching any exception that may happen during execution.
    /// </summary>
    /// <typeparam name="A">Type returned by the delegate.</typeparam>
    /// <param name="fn">Delegate to execute.</param>
    /// <param name="onException">An action to execute upon catching an exception.</param>
    protected async Task<A> CallAsync<A>(Func<Task<A>> fn, Action<Exception> onException = null)
    {
      try
      {
        IsBusy = true;
        return await fn();
      }
      catch (Exception ex)
      {
        onException?.Invoke(ex);
        return default(A);
      }
      finally
      {
        IsBusy = false;
      }
    }

    /// <summary>
    /// Executes a delegate that returns a value of the given type asynchronously, 
    /// catching any exception that may happen during execution.
    /// </summary>
    /// <param name="fn">Delegate to execute.</param>
    /// <param name="onException">An action to execute upon catching an exception.</param>
    protected async Task CallAsync(Func<Task> fn, Action<Exception> onException = null)
    {
      // TODO: do a check if isBusy first maybe? Then I wouldn't need to Observe it on every command.
      try
      {
        IsBusy = true;
        await fn();
      }
      catch (Exception ex)
      {
        onException?.Invoke(ex);
        // TODO:
        // maybe if null, invoke (abstracted here) HandleException method
      }
      finally
      {
        IsBusy = false;
      }
    }
  }
}
