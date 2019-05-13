﻿using IdentityServer4.EntityFramework.Entities;
using IdS4.Infrastructure.DbContexts;
using IdS4.Wrappers;
using RajsLibs.EfCore.Uow;
using RajsLibs.Repository.EfCore;

namespace IdS4.Repositories
{
    public class PersistedGrantRepository : RepositoryBase<IdS4PersistedGrantDbContext, IdS4PersistedGrant, string>, IPersistedGrantRepository
    {
        public PersistedGrantRepository(IEfUnitOfWork<IdS4PersistedGrantDbContext> unitOfWork) : base(unitOfWork)
        {

        }
    }
}