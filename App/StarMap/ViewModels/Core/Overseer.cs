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

    /// <summary>
    /// Executes a delegate that returns void, 
    /// catching any exceptions that may happen during the execution.
    /// </summary>
    /// <remarks>
    /// Although the method call is executed synchronously, it is marked async, since 
    /// global error handling is an asynchronous operation.
    /// </remarks>
    /// <param name="fn">a delegate to execute.</param>
    protected async Task Call(Action fn)
    {
      try
      {
        IsBusy = true;
        fn();
      }
      catch (Exception ex)
      {
        await HandleException(ex);
      }
      finally
      {
        IsBusy = false;
      }
    }

    /// <summary>
    /// Executes a delegate that returns a value of the given type, 
    /// catching any exception that may happen during the execution.
    /// </summary>
    /// <remarks>
    /// Although the method call is executed synchronously, it is marked async, since 
    /// global error handling is an asynchronous operation.
    /// </remarks>
    /// <param name="fn">Delegate to execute.</param>
    protected async Task<A> Call<A>(Func<A> fn)
    {
      try
      {
        IsBusy = true;
        return fn();
      }
      catch (Exception ex)
      {
        await HandleException(ex);

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
    protected async Task CallAsync(Func<Task> fn, Func<Exception, Task> onException = null)
    {
      try
      {
        IsBusy = true;
        await fn();
      }
      catch (Exception ex)
      {
        await HandleException(ex);
        //if (onException != null)
        //  await onException(ex);
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
        await HandleException(ex);
        //if (onException != null)
        //  await onException(ex);

        return default(A);
      }
      finally
      {
        IsBusy = false;
      }
    }
  }
}
