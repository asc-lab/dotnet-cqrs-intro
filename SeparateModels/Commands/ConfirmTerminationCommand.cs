using MediatR;

namespace SeparateModels.Commands
{
    public class ConfirmTerminationCommand : IRequest<ConfirmTerminationResult>
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