using SQLite;
using StarMap.Cll.Abstractions.Providers;
using System;
using System.Threading.Tasks;

namespace StarMap.Dal.Providers
{
    public abstract class DatabaseAsyncProvider
    {
        private readonly IConnectionProvider _connectionProvider;

        protected DatabaseAsyncProvider(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        /// <summary>
        /// Grants access to the <see cref="SQLiteAsyncConnection"/>
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="action">A read action to perform on database through the <see cref="SSQLiteAsyncConnection"/>.</param>
        /// <returns>The value taken from DB.</returns>
        protected async Task<T> Read<T>(Func<SQLiteAsyncConnection, Task<T>> action)
        {
            var connection = _connectionProvider.GetConnection();
            try
            {
                return await action(connection).ConfigureAwait(false);
            }
            catch
            {
                throw new Exception("Database access exception");
            }
            finally
            {
                await connection.CloseAsync().ConfigureAwait(false);
            }
        }
    }
}
