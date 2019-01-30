namespace NoCqrs.Services
{
    public class ConfirmBuyAdditionalCoverRequest
    {
        public string PolicyNumber { get; set; }
        public int VersionToConfirmNumber { get; set; }
    }

    public class ConfirmBuyAdditionalCoverResult
    {
        public string PolicyNumber { get; set; }
        public int VersionConfirmed { get; set; }
    }
}