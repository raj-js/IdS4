using RajsLibs.DI;
using System;
using System.Threading.Tasks;

namespace RajsLibs.Uow
{
    public interface IUnitOfWorkManager : IScopeObject, IDisposable
    {
        void Commit();

        Task CommitAsync();

        void Register(IUnitOfWork unitOfWork);
    }
}
