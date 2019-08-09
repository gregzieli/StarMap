using SQLite;
using StarMap.Cll.Abstractions;
using StarMap.Cll.Abstractions.Providers;

namespace StarMap.Dal.Providers
{
    public class ConnectionProvider : IConnectionProvider
    {
        private readonly IRepository _repository;

        public ConnectionProvider(IRepository repository)
        {
            _repository = repository;
        }

        public SQLiteAsyncConnection GetConnection()
        {
            return new SQLiteAsyncConnection(_repository.GetFilePath(), SQLiteOpenFlags.ReadOnly);
        }
    }
}
