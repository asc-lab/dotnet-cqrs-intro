namespace NoCqrs.Services
{
    public class CancelLastAnnexRequest
    {
        public string PolicyNumber { get; set; }
    }

    public class CancelLastAnnexResult
    {
        public string PolicyNumber { get; set; }
        public int LastActiveVersionNumber { get; set; }
    }
}