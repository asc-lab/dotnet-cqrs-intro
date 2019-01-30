using System;
using System.Collections.Generic;
using MediatR;
using SeparateModels.Services;

namespace SeparateModels.Queries
{
    public class FindPoliciesQuery : IRequest<List<PolicyInfoDto>>
    {
        public string PolicyNumber { get; set; }
        public DateTime? PolicyStartFrom { get; set; }
        public DateTime? PolicyStartTo { get; set; }
        public string CarPlateNumber { get; set; }
        public string PolicyHolderFirstName { get; set; }
        public string PolicyHolderLastName { get; set; }
    }
}