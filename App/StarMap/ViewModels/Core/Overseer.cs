using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarMap.ViewModels.Core
{
  public abstract class Overseer : BindableBase
  {
    private bool _isBusy = false;
    public bool IsBusy
    {
      get { return _isBusy; }
      set { SetProperty(ref _isBusy, value); }
    }

    /// <summary>
    /// Ensures that no command is executed when a VM is busy.
    /// </summary>
    /// <returns>true, if not busy; false otherwise.</returns>
    protected virtual bool CanExecute() => !IsBusy;

    protected abstract Task HandleException(Exception ex);

    #region Non-async (REMOVE?)
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
        IsBusy = true;
        fn();
      }
      catch (Exception ex)
      {
        onException?.Invoke(ex);
      }
      finally
      {
        IsBusy = false;
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
        IsBusy = true;
        return fn();
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

    #endregion

    
    /// <summary>
    /// Executes a delegate that returns a value of the given type asynchronously, 
    /// catching any exception that may happen during execution.
    /// </summary>
    /// <param name="fn">Delegate to execute.</param>
    /// <param name="onException">An action to execute upon catching an exception.</param>
    protected async Task CallAsync(Func<Task> fn, Func<Exception, Task> onException = null)
    {
      try
      {
        IsBusy = true;
        await fn();
      }
      catch (Exception ex)
      {
        //if (onException != null)
        //  await onException(ex).ConfigureAwait(continueOnCapturedContext: false);
        await HandleException(ex);
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
    protected async Task<A> CallAsync<A>(Func<Task<A>> fn, Func<Exception, Task> onException = null)
    {
      try
      {
        IsBusy = true;
        return await fn();
      }
      catch (Exception ex)
      {
        await HandleException(ex).ConfigureAwait(continueOnCapturedContext: false);
        //if (onException != null)
        //  await onException(ex).ConfigureAwait(continueOnCapturedContext: false);

        return default(A);
      }
      finally
      {
        IsBusy = false;
      }
    }
  }
}
