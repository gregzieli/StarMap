using System;
using System.Threading.Tasks;

namespace StarMap.Bll.Managers
{
  public abstract class BaseManager
  {

    /// <summary>
    /// Executes a delegate that returns a value of the given type, 
    /// catching any exception that may happen during execution.
    /// </summary>
    /// <typeparam name="A">Type returned by the delegate.</typeparam>
    /// <param name="fn">Delegate to execute.</param>
    /// <param name="suppress">Whether to supress the exception or not.</param>
    /// <param name="onException">An action to execute upon catching an exception.</param>
    protected A Call<A>(Func<A> fn, bool suppress = false, Action<Exception> onException = null)
    {
      try
      {
        return fn();
      }
      catch (Exception ex)
      {
        onException?.Invoke(ex);
        return suppress ? default(A) : throw ex;
      }
    }

    /// <summary>
    /// Executes a delegate that returns a value of the given type asynchronously, 
    /// catching any exception that may happen during execution.
    /// </summary>
    /// <typeparam name="A">Type returned by the delegate.</typeparam>
    /// <param name="fn">Delegate to execute.</param>
    /// <param name="suppress">Whether to supress the exception or not.</param>
    /// <param name="onException">An action to execute upon catching an exception.</param>
    protected async Task<A> CallAsync<A>(Func<Task<A>> fn, bool suppress = false, Action<Exception> onException = null)
    {
      try
      {
        return await fn();
      }
      catch (Exception ex)
      {
        onException?.Invoke(ex);
        return suppress ? default(A) : throw ex;
      }
    }

    /// <summary>
    /// Executes a delegate catching any exception that may happen during execution.
    /// </summary>
    /// <param name="fn">Delegate to execute.</param>
    /// <param name="suppress">Whether to supress the exception or not.</param>
    /// <param name="onException">An action to execute upon catching an exception.</param>
    protected void Call(Action fn, bool suppress = false, Action<Exception> onException = null)
    {
      try
      {
        fn();
      }
      catch (Exception ex)
      {
        onException?.Invoke(ex);
        if (suppress)
          return;

        throw ex;
      }
    }
  }
}
