namespace TicketCall.Api.Entities;

using System.ComponentModel.DataAnnotations;
using TicketCall.Api.Entities.Enums;

public class Ticket
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;
    public Status Status { get; set; }
    public Priority Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
