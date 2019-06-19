using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using IdS4.Application.Commands;
using IdS4.Application.Models.Resource;
using IdS4.DbContexts;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IdS4.Application.CommandHandlers
{
    public class AddApiResourceCommandHandler :
        IRequestHandler<AddApiResourceCommand, VmApiResource>
    {
        private readonly IdS4ConfigurationDbContext _configurationDb;
        private readonly IMapper _mapper;

        public AddApiResourceCommandHandler(IdS4ConfigurationDbContext configurationDb, IMapper mapper)
        {
            _configurationDb = configurationDb;
            _mapper = mapper;
        }

        public async Task<VmApiResource> Handle(AddApiResourceCommand request, CancellationToken cancellationToken)
        {
            var resource = _mapper.Map<ApiResource>(request.Resource);

            var entry = await _configurationDb.ApiResources.AddAsync(resource, cancellationToken);
            await _configurationDb.SaveChangesAsync(cancellationToken);

            return _mapper.Map<VmApiResource>(entry.Entity);
        }
    }
}
