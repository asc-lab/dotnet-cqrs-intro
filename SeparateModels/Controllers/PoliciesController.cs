using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SeparateModels.Commands;
using SeparateModels.Queries;
using SeparateModels.Services;

namespace SeparateModels.Controllers
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
        
        [HttpPost("CancelLastAnnex")]
        public async Task<IActionResult> CancelLastAnnex([FromBody] CancelLastAnnexCommand cmd)
        {
            var result = await mediator.Send(cmd);
            return Ok(result);
        }

        [HttpGet("{policyNumber}/versions")]
        public async Task<IActionResult> GetPolicyVersionsList(string policyNumber)
        {
            var result = await mediator.Send(new GetPolicyVersionsListQuery { PolicyNumber = policyNumber });
            return Ok(result);
        }
        
        [HttpGet("{policyNumber}/versions/{versionNumber}")]
        public async Task<IActionResult> GetPolicyVersionDetails(string policyNumber, int versionNumber)
        {
            var result = await mediator.Send(new GetPolicyVersionDetailsQuery { PolicyNumber = policyNumber, VersionNumber = versionNumber});
            return Ok(result);
        }


        [HttpPost("find")]
        public async Task<IActionResult> Find([FromBody] FindPoliciesQuery query)
        {
            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}