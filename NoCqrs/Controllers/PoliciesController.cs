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
            var response = policyService.CreatePolicy(createPolicyRequest);
            return Ok(response);
        }



        [HttpPost("/find")]
        public IActionResult Find([FromBody] SearchPolicyRequest searchPolicyRequest)
        {
            var response = policyService.SearchPolicies(searchPolicyRequest);
            return Ok(response);
        }
    }
}