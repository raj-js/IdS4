using System;
using System.Threading.Tasks;

namespace RajsLibs.Uow
{
    public interface IUnitOfWork : IDisposable
    {
        int Commit();

        Task<int> CommitAsync();
    }
}
