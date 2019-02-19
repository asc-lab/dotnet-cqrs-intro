using System.Threading.Tasks;
using CqrsWithEs.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CqrsWithEs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoliciesController : ControllerBase
    {
        private readonly IMediator mediator;

        public PoliciesController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        
        [HttpPost]
        public async Task<IActionResult> CreatePolicy([FromBody] CreatePolicyCommand cmd)
        {
            var result = await mediator.Send(cmd);
            return Ok(result);
        }
        
        [HttpPost("BuyAdditionalCover")]
        public async Task<IActionResult> BuyAdditionalCover([FromBody] BuyAdditionalCoverCommand cmd)
        {
            var result = await mediator.Send(cmd);
            return Ok(result);
        }
        
        [HttpPost("ConfirmBuyAdditionalCover")]
        public async Task<IActionResult> Post([FromBody] ConfirmBuyAdditionalCoverCommand cmd)
        {
            var result = await mediator.Send(cmd);
            return Ok(result);
        }
        
        [HttpPost("Terminate")]
        public async Task<IActionResult> Terminate([FromBody] TerminatePolicyCommand cmd)
        {
            var result = await mediator.Send(cmd);
            return Ok(result);
        }
        
        [HttpPost("ConfirmTermination")]
        public async Task<IActionResult> ConfirmTermination([FromBody] ConfirmTerminationCommand cmd)
        {
            var result = await mediator.Send(cmd);
            return Ok(result);
        }

    }
}