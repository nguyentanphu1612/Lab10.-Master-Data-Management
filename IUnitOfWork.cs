using System;
using System.Threading.Tasks;

namespace ASC.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>() where T : class;
        Task<int> CommitAsync();
        Task RollbackAsync();
    }
}