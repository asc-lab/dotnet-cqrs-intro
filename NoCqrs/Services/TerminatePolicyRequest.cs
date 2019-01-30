using System;

namespace NoCqrs.Services
{
    public class TerminatePolicyRequest
    {
        public string PolicyNumber { get; set; }
        public DateTime TerminationDate { get; set; }
    }

    public class TerminatePolicyResult
    {
        public string PolicyNumber { get; set; }
        public int VersionWithTerminationNumber { get; set; }
    }
}