using Microsoft.AspNetCore.Mvc;
using NoCqrs.Services;

namespace NoCqrs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoliciesController : ControllerBase
    {
        private readonly PolicyService policyService;

        public PoliciesController(PolicyService policyService)
        {
            this.policyService = policyService;
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] CreatePolicyRequest createPolicyRequest)
        {
            var result = policyService.CreatePolicy(createPolicyRequest);
            return Ok(result);
        }

        [HttpGet("/{policyNumber}")]
        public IActionResult GetPolicyDetails(string policyNumber)
        {
            var result = policyService.GetPolicyDetails(policyNumber);
            return Ok(result);
        }


        [HttpPost("/find")]
        public IActionResult Find([FromBody] SearchPolicyRequest searchPolicyRequest)
        {
            var result = policyService.SearchPolicies(searchPolicyRequest);
            return Ok(result);
        }
    }
}