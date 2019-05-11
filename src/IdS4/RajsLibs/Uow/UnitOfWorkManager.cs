using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RajsLibs.Uow
{
    public class UnitOfWorkManager : IUnitOfWorkManager
    {
        private readonly List<IUnitOfWork> _unitOfWorks;

        public UnitOfWorkManager()
        {
            _unitOfWorks = new List<IUnitOfWork>();
        }

        public void Commit()
        {
            foreach (var unitOfWork in _unitOfWorks)
                unitOfWork.Commit();
        }

        public async Task CommitAsync(CancellationToken cancallationToken = default(CancellationToken))
        {
            foreach (var unitOfWork in _unitOfWorks)
                await unitOfWork.CommitAsync(cancallationToken);
        }

        public void Dispose()
        {
            foreach (var unitOfWork in _unitOfWorks)
                unitOfWork?.Dispose();
        }

        public void Register(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            if (!_unitOfWorks.Contains(unitOfWork))
                _unitOfWorks.Add(unitOfWork);
        }
    }
}
