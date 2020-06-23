
using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using ProjectName.DomainName1.Application.Commands;

namespace ProjectName.Web.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class HomeController : BaseController
    {
        public HomeController(IServiceProvider provider)
            : base(provider)
        { }
        
        [HttpGet]
        public async Task<IActionResult> Index(string name = "Praticis")
        {
            var result = await this.ServiceBus.SendCommand(new ResponseCommand(name));

            return Response(result);
        }
        
        /*
        OR

        [HttpGet]
        public async Task<IActionResult> Index(HelloViewModel hello)
        {
            var result = await this._serviceBus.SendCommand(new ResponseCommand(hello));

            return Response(result);
        }
        */
        
        [HttpGet]
        public async Task<IActionResult> DefaultCommand(long sleepTime = 1000)
        {
            await this.ServiceBus.SendCommand(new DefaultCommand(TimeSpan.FromMilliseconds(sleepTime)));

            return Response();
        }
    }
}