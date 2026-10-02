using Kepas.Core.Api.Domain.Enums;

namespace Kepas.Core.Api.Domain.DTOs.Requests.ServiceAccounts
{
    public class UpdatePipelineRequest
    {
        public PipelineStatus Status { get; set; }
        public decimal EstimatedValue { get; set; }
    }
}
