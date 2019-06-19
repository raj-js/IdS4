using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using IdS4.Application.Commands;
using IdS4.Application.Models.Client;
using IdS4.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdS4.Application.CommandHandlers
{
    class EditClientConsentCommandHandler : IRequestHandler<EditClientConsentCommand, VmClient.Consent>
    {
        private readonly IdS4ConfigurationDbContext _configurationDb;
        private readonly IMapper _mapper;

        public EditClientConsentCommandHandler(IdS4ConfigurationDbContext configurationDb, IMapper mapper)
        {
            _configurationDb = configurationDb;
            _mapper = mapper;
        }

        public async Task<VmClient.Consent> Handle(EditClientConsentCommand request, CancellationToken cancellationToken)
        {
            var client = await _configurationDb.Clients.AsNoTracking().SingleOrDefaultAsync(s => s.Id == request.Consent.Id, cancellationToken);
            if (client == null) return null;

            var vm = _mapper.Map<VmClient>(client);
            request.Consent.ApplyChangeToClient(vm);

            var entity = _mapper.Map<Client>(vm);
            var entry = _configurationDb.Attach(entity);
            entry.State = EntityState.Modified;
            await _configurationDb.SaveChangesAsync();

            vm = _mapper.Map<VmClient>(entity);
            return vm.ToConsent(_mapper);
        }
    }
}
