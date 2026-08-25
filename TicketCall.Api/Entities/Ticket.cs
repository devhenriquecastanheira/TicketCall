namespace TicketCall.Api.Entities;
using TicketCall.Api.Entities.Enums;

public class Ticket
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Status Status { get; set; }
    public Priority Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
