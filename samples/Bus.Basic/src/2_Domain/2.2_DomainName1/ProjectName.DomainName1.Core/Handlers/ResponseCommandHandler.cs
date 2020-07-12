
using System.Threading;
using System.Threading.Tasks;

using Praticis.Framework.Bus.Abstractions.Commands;
using ProjectName.DomainName1.Application.Commands;

namespace ProjectName.DomainName1.Core.Handlers
{
    public class ResponseCommandHandler : ICommandHandler<ResponseCommand, string>
    {
        public Task<string> Handle(ResponseCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult($"Hello, {request.Hello.Name}");
        }

        public void Dispose()
        {
            
        }
    }
}
