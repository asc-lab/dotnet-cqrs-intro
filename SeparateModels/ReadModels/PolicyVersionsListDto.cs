using System;
using System.Collections.Generic;

namespace SeparateModels.ReadModels
{
    public class PolicyVersionsListDto
    {
        public string PolicyNumber { get; set; }
        public List<PolicyVersionInfoDto> VersionsInfo { get; set; }
    }

    public class PolicyVersionInfoDto
    {
        public int Number { get; set; }
        public DateTime VersionFrom { get; set; }
        public DateTime VersionTo { get; set; }
        public string VersionStatus { get; set; }
    }

}