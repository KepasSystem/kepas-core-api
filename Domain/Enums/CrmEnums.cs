namespace Kepas.Core.Api.Domain.Enums
{
    public enum PipelineStatus
    {
        Prospect = 0,
        InNegotiation = 1,
        Active = 2,
        Churned = 3
    }

    public enum InteractionType
    {
        Note = 0,
        Email = 1,
        Call = 2,
        Meeting = 3
    }

    public enum DocumentType
    {
        Contract = 0,
        Identification = 1,
        Other = 2
    }

    public enum TicketStatus
    {
        Open = 0,
        InProgress = 1,
        Resolved = 2,
        Closed = 3
    }

    public enum TicketPriority
    {
        Low = 0,
        Medium = 1,
        High = 2,
        Critical = 3
    }
}
