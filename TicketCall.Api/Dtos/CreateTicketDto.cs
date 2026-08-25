using System.ComponentModel.DataAnnotations;
using TicketCall.Api.Entities.Enums;

namespace TicketCall.Api.Dtos;

public class CreateTicketDto
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [StringLength(100)]
    public string Description { get; set; } = string.Empty;
    public Status Status { get; set; }
    public Priority Priority { get; set; }
}
