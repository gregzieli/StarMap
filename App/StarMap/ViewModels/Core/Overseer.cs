using Prism.Mvvm;
using System;
using System.Threading.Tasks;

namespace StarMap.ViewModels.Core
{
  public abstract class Overseer : BindableBase
  {
    private bool _isBusy;
    public bool IsBusy
    {
      get { return _isBusy; }
      set { SetProperty(ref _isBusy, value); }
    }

    /// <summary>
    /// Ensures that no command is executed when the current VM is busy. Can be overriden.
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
    /// Executes a delegate that returns a value of the given type asynchronously, 
    /// catching any exception that may happen during execution.
    /// </summary>
    /// <param name="fn">Delegate to execute.</param>
    /// <param name="onException">An action to execute upon catching an exception.</param>
    protected async Task CallAsync(Func<Task> fn)
    {
      try
      {
        IsBusy = true;
        await fn();
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
    /// Calls an asynchronous method and executes the callback using the result.
    /// </summary>
    /// <typeparam name="A">Return type of the async call.</typeparam>
    /// <param name="fn">Asynchronous delegate.</param>
    /// <param name="callback">Delegate that operates on the data awaited.</param>
    protected async Task CallAsync<A>(Func<Task<A>> fn, Action<A> callback)
    {
      try
      {
        IsBusy = true;
        var result = await fn();
        // In simple cases, instead of awaiting a long lambda (that needs the async await keywords),
        // the call could be broken into two parameters.
        callback(result);
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

    protected async Task CallAsync(Func<Task> fn, Action callback)
    {
      try
      {
        IsBusy = true;
        await fn();
        // just to keep the synchronous code in the try block.
        callback();
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
  }
}
