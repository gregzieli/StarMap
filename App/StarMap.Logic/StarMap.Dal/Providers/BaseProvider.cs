using System;

namespace StarMap.Dal.Providers
{
  public abstract class BaseProvider<C> where C : IDisposable
  {
    /// <summary>
    /// Gets a context for this BaseProvider.
    /// </summary>
    protected abstract C Context { get; }

    static readonly object _locker = new object();

    protected R Read<R>(Func<C, R> fn)
    {
      R data = default(R);
      try
      {
        lock (_locker)
        {
          using (var context = Context)
          {
            data = fn(context);
          } 
        }
      }
      catch (Exception e)
      {
        // TODO: maybe throw a wrapper custom exc.
        throw e;
      }
      return data;
    }
  }
}
