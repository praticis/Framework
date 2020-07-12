
using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using ProjectName.DomainName1.Application.Commands.CustomerCommands;
using ProjectName.DomainName1.Application.ViewModels;

namespace ProjectName.Web.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CustomersController : BaseController
    {
        public CustomersController(IServiceProvider provider)
            : base(provider)
        { }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await this.ServiceBus.SendCommand(new LoadCustomersCommand());

            return Response(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> Save(CustomerViewModel customer)
        {
            await this.ServiceBus.SendCommand(new SaveCustomerCommand(customer));

            return Response();
        }
    }
}