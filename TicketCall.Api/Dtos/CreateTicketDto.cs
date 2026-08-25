using TicketCall.Api.Entities.Enums;

namespace TicketCall.Api.Dtos;

public class CreateTicketDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Status Status { get; set; }
    public Priority Priority { get; set; }
}
