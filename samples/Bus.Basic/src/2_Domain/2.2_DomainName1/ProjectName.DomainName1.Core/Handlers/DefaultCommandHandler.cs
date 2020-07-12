
using System;
using System.Threading;
using System.Threading.Tasks;

using Praticis.Framework.Bus.Abstractions;
using Praticis.Framework.Bus.Abstractions.Commands;

using ProjectName.DomainName1.Application.Commands;

namespace ProjectName.DomainName1.Core.Handlers
{
    public class DefaultCommandHandler : ICommandHandler<DefaultCommand>
    {
        private readonly IServiceBus _serviceBus;

        public DefaultCommandHandler(IServiceProvider provider)
        {
            this._serviceBus = provider.GetService<IServiceBus>();
        }
        public async Task<bool> Handle(DefaultCommand request, CancellationToken cancellationToken)
        {
            Thread.Sleep(request.SleepTime);

            string msg = $"{typeof(DefaultCommand).Name} worked by {request.SleepTime.TotalMilliseconds} ms";

            await this._serviceBus.PublishEvent(new Log(msg));

            return true;
        }

        public void Dispose()
        {
            
        }
    }
}