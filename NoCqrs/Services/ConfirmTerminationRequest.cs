namespace NoCqrs.Services
{
    public class ConfirmTerminationRequest
    {
        public string PolicyNumber { get; set; }
        public int VersionToConfirmNumber { get; set; }
    }

    public class ConfirmTerminationResult
    {
        public string PolicyNumber { get; set; }
        public int VersionConfirmed { get; set; }
    }
}