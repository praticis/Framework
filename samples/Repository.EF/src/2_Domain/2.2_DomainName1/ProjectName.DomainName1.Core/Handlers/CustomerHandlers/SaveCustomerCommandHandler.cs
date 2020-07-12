
using System;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using Praticis.Framework.Bus.Abstractions;
using Praticis.Framework.Bus.Abstractions.Commands;
using ProjectName.DomainName1.Application.Commands.CustomerCommands;
using ProjectName.DomainName1.Domain.Interfaces.Repositories;
using ProjectName.DomainName1.Domain.Models;

namespace ProjectName.DomainName1.Core.Handlers.CustomerHandlers
{
    public class SaveCustomerCommandHandler : ICommandHandler<SaveCustomerCommand>
    {
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _customerRepository;
        private readonly IServiceBus _serviceBus;

        public SaveCustomerCommandHandler(IServiceProvider provider)
        {
            this._mapper = provider.GetService<IMapper>();
            this._customerRepository = provider.GetService<ICustomerRepository>();
            this._serviceBus = provider.GetService<IServiceBus>();
        }

        public async Task<bool> Handle(SaveCustomerCommand request, CancellationToken cancellationToken)
        {
            bool saved;
            Customer customer;

            var result = await this._customerRepository.GetAllAsync(new Praticis.Framework.Layers.Data.Abstractions.Filters.BasePaginationFilter());

            customer = this._mapper.Map<Customer>(request.Customer);
            
            // your business logic here...

            await this._customerRepository.SaveAsync(customer);

            saved = await this._customerRepository.CommitAsync();

            return saved && !this._serviceBus.Notifications.HasNotifications();
        }

        public void Dispose()
        {
            
        }
    }
}