using IdentityServer4.EntityFramework.Entities;
using IdS4.Application.Interface;
using IdS4.Application.Paging;
using IdS4.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace IdS4.Application.Implementation
{
    public class IdentityResourceService : IIdentityResourceService
    {
        private readonly IdS4ConfigurationDbContext _configurationDb;

        public IdentityResourceService(IdS4ConfigurationDbContext configurationDb)
        {
            _configurationDb = configurationDb ?? throw new ArgumentNullException(nameof(configurationDb));
        }

        public async Task AddAsync(IdentityResource identityResource)
        {
            if (identityResource == null)
                throw new ArgumentNullException(nameof(identityResource));

            await _configurationDb.IdentityResources.AddAsync(identityResource);
            await _configurationDb.SaveChangesAsync();
        }

        public async Task ModifyAsync(IdentityResource identityResource)
        {
            if (identityResource == null)
                throw new ArgumentNullException(nameof(identityResource));

            _configurationDb.IdentityResources.Update(identityResource);
            await _configurationDb.SaveChangesAsync();
        }

        public async Task DeleteAsync(IdentityResource identityResource)
        {
            if (identityResource == null)
                throw new ArgumentNullException(nameof(identityResource));

            _configurationDb.IdentityResources.Remove(identityResource);
            await _configurationDb.SaveChangesAsync();
        }

        public async Task<IdentityResource> FindAsync(int id)
        {
            return await _configurationDb.IdentityResources.FindAsync(id);
        }

        public async Task<Paged<IdentityResource>> SearchAsync(PageQuery<IdentityResource> query)
        {
            var d = await _configurationDb.IdentityResources
                .AsNoTracking()
                .Where(query.Filters)
                .OrderBy(query.OrderBy)
                .Skip(query.Skip)
                .Take(query.PageSize)
                .ToListAsync();

            var c = await _configurationDb.IdentityResources.CountAsync();

            return new Paged<IdentityResource> { Data = d, TotalCount = c };
        }
    }
}
