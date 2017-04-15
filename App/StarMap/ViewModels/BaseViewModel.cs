using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarMap.ViewModels
{
  public class BaseViewModel : BindableBase
  {
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
        return await fn();
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
    /// <param name="fn">Delegate to execute.</param>
    /// <param name="onException">An action to execute upon catching an exception.</param>
    protected async Task CallAsync(Func<Task> fn, Action<Exception> onException = null)
    {
      try
      {
        await fn();
      }
      catch (Exception ex)
      {
        onException?.Invoke(ex);
        return;
      }
    }
  }
}
