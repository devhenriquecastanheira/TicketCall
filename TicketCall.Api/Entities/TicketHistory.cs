using TicketCall.Api.Entities.Enums;

namespace TicketCall.Api.Entities;

public class TicketHistory
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public Ticket Ticket { get; set; }
    public Status OldStatus { get; set; }
    public Status NewStatus { get; set; }
    public DateTime ChangedAt { get; set; }
}
