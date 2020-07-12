
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using Praticis.Framework.Bus.Abstractions.Commands;

using ProjectName.DomainName1.Application.Commands.CustomerCommands;
using ProjectName.DomainName1.Application.ViewModels;
using ProjectName.DomainName1.Domain.Interfaces.Repositories;

namespace ProjectName.DomainName1.Core.Handlers.CustomerHandlers
{
    public class LoadCustomersCommandHandler : ICommandHandler<LoadCustomersCommand, IEnumerable<CustomerViewModel>>
    {
        private readonly IMapper _mapper;
        private ICustomerReadRepository _customerRepository { get; set; }

        public LoadCustomersCommandHandler(IServiceProvider provider)
        {
            this._mapper = provider.GetService<IMapper>();
            this._customerRepository = provider.GetService<ICustomerReadRepository>();
        }

        public async Task<IEnumerable<CustomerViewModel>> Handle(LoadCustomersCommand request, CancellationToken cancellationToken)
        {
            var customers = await this._customerRepository.GetAllAsync(f => f.PageSize = 1);
            
            return this._mapper.Map<IEnumerable<CustomerViewModel>>(customers);
        }

        public void Dispose()
        {
            
        }
    }
}