using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IdS4.Application.Commands;
using IdS4.Application.Models.Resource;
using IdS4.DbContexts;
using MediatR;

namespace IdS4.Application.CommandHandlers
{
    public class EditApiResourceCommandHandler:
        IRequestHandler<EditApiResourceCommand, VmApiResource>
    {
        private readonly IdS4ConfigurationDbContext _configurationDb;
        private readonly IMapper _mapper;

        public EditApiResourceCommandHandler(IdS4ConfigurationDbContext configurationDb, IMapper mapper)
        {
            _configurationDb = configurationDb;
            _mapper = mapper;
        }

        public async Task<VmApiResource> Handle(EditApiResourceCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
