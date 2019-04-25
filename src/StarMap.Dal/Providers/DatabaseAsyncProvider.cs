using SQLite;
using StarMap.Cll.Abstractions;
using System;
using System.Threading.Tasks;

namespace StarMap.Dal.Providers
{
    public abstract class DatabaseAsyncProvider
    {
        private readonly SQLiteAsyncConnection _connection;

        protected DatabaseAsyncProvider(IRepository repository)
        {
            _connection = new SQLiteAsyncConnection(repository.GetFilePath(), SQLiteOpenFlags.ReadOnly);
        }

        /// <summary>
        /// Grants access to the <see cref="SQLiteAsyncConnection"/>
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="action">A read action to perform on database through the <see cref="SSQLiteAsyncConnection"/>.</param>
        /// <returns>The value taken from DB.</returns>
        protected async Task<T> Read<T>(Func<SQLiteAsyncConnection, Task<T>> action)
        {
            try
            {
                return await action(_connection).ConfigureAwait(false);
            }
            catch
            {
                throw new Exception("Database access exception");
            }
            finally
            {
                await _connection.CloseAsync().ConfigureAwait(false);
            }
        }
    }
}
