using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RajsLibs.Uow
{
    public abstract class UnitOfWorkManagerBase : IUnitOfWorkManager
    {
        protected List<IUnitOfWork> UnitOfWorks;

        public UnitOfWorkManagerBase()
        {
            UnitOfWorks = new List<IUnitOfWork>();
        }

        public virtual void Commit()
        {
            foreach (var unitOfWork in UnitOfWorks)
                unitOfWork.Commit();
        }

        public virtual async Task CommitAsync(CancellationToken cancallationToken = default(CancellationToken))
        {
            foreach (var unitOfWork in UnitOfWorks)
                await unitOfWork.CommitAsync(cancallationToken);
        }

        public virtual void Dispose()
        {
            foreach (var unitOfWork in UnitOfWorks)
                unitOfWork?.Dispose();
        }

        public virtual void Register(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            if (!UnitOfWorks.Contains(unitOfWork))
                UnitOfWorks.Add(unitOfWork);
        }
    }
}
