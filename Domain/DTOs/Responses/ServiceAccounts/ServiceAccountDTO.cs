using System;
using System.Collections.Generic;

namespace Kepas.Core.Api.Domain.DTOs.Responses.ServiceAccounts
{
    public class ServiceAccountDTO
    {
        public Guid Id { get; set; }
        public string OwnerName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TotalTenants { get; set; }
        public int TotalSubscriptions { get; set; }
        public Domain.Enums.PipelineStatus PipelineStatus { get; set; }
        public decimal EstimatedValue { get; set; }
    }
}

