using RajsLibs.DI;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RajsLibs.Uow
{
    public interface IUnitOfWorkManager : IScopeObject, IDisposable
    {
        void Commit();

        Task CommitAsync(CancellationToken cancallationToken = default(CancellationToken));

        void Register(IUnitOfWork unitOfWork);
    }
}
