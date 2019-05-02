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
            get => _isBusy;
            set => SetProperty(ref _isBusy, value, onChanged: () => RaisePropertyChanged(nameof(CanExecute)));
        }

        /// <summary>
        /// Ensures that no command is executed when the current VM is busy. Can be overriden.
        /// </summary>
        /// <returns>true, if not busy; false otherwise.</returns>
        protected virtual bool CanExecute => !_isBusy;

        protected abstract Task HandleException(Exception ex);

        /// <summary>
        /// Executes a delegate that returns void, 
        /// catching any exceptions that may happen during the execution.
        /// </summary>
        /// <remarks>
        /// Although the method call is executed synchronously, it is marked async, since 
        /// global error handling is an asynchronous operation.
        /// </remarks>
        /// <param name="fn">A delegate to execute.</param>
        /// <param name="always">An action to execute always, no matter if an error occured or not.</param>
        protected async Task Call(Action fn, Action always = null)
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
                Finalize(always);
            }
        }

        /// <summary>
        /// Executes a delegate that returns a value of the given type asynchronously, 
        /// catching any exception that may happen during execution.
        /// </summary>
        /// <param name="fn">Delegate to execute.</param>
        /// <param name="onException">An action to execute upon catching an exception.</param>
        /// <param name="onDone">An action to execute after the asynchronous code completes.</param>
        /// <param name="always">An action to execute always, no matter if an error occured or not.</param>
        protected async Task CallAsync(Func<Task> fn, Action onDone = null, Action always = null)
        {
            try
            {
                IsBusy = true;
                await fn();
                onDone?.Invoke();
            }
            catch (Exception ex)
            {
                await HandleException(ex);
            }
            finally
            {
                Finalize(always);
            }
        }

        /// <summary>
        /// Calls an asynchronous method and executes the callback using the result.
        /// </summary>
        /// <typeparam name="TResult">Return type of the async call.</typeparam>
        /// <param name="fn">Asynchronous delegate.</param>
        /// <param name="onDone">Delegate that operates on the data awaited.</param>
        /// <param name="always">An action to execute always, no matter if an error occured or not.</param>
        protected async Task CallAsync<TResult>(Func<Task<TResult>> fn, Action<TResult> onDone, Action always = null)
        {
            try
            {
                IsBusy = true;
                var result = await fn();
                // In simple cases, instead of awaiting a long lambda (that needs the async await keywords),
                // the call could be broken into two parameters.
                onDone(result);
            }
            catch (Exception ex)
            {
                await HandleException(ex);
            }
            finally
            {
                Finalize(always);
            }
        }

        private void Finalize(Action always = null)
        {
            always?.Invoke();
            IsBusy = false;
        }
    }
}
