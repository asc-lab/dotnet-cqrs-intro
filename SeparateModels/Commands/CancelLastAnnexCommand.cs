using MediatR;

namespace SeparateModels.Services
{
    public class CancelLastAnnexCommand : IRequest<CancelLastAnnexResult>
    {
        public string PolicyNumber { get; set; }
    }

    public class CancelLastAnnexResult
    {
        public string PolicyNumber { get; set; }
        public int LastActiveVersionNumber { get; set; }
    }
}