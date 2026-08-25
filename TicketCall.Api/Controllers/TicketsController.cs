using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketCall.Api.Data;
using TicketCall.Api.Dtos;
using TicketCall.Api.Entities;
using TicketCall.Api.Entities.Enums;

namespace TicketCall.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TicketsController : ControllerBase
{
    private readonly AppDbContext _context;

    public TicketsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets(
        [FromQuery] Status? status,
        [FromQuery] Priority? priority,
        [FromQuery] string? search)
    {
        var tickets = _context.Tickets.AsQueryable();

        if (status.HasValue)
        {
            tickets = tickets.Where(t => t.Status == status.Value);
        }

        if (priority.HasValue)
        {
            tickets = tickets.Where(t => t.Priority == priority.Value);
        }

        if (!string.IsNullOrEmpty(search))
        {
            tickets = tickets.Where(t => t.Title.Contains(search) || t.Description.Contains(search));
        }

        return await tickets.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Ticket>> GetTicket(int id)
    {
        var ticket = await _context.Tickets.FindAsync(id);

        if (ticket == null)
        {
            return NotFound();
        }

        return ticket;
    }

    [HttpPost]
    public async Task<ActionResult<Ticket>> CreateTicket(CreateTicketDto dto)
    {
        var ticket = new Ticket
        {
            Title = dto.Title,
            Description = dto.Description,
            Status = dto.Status,
            Priority = dto.Priority,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, ticket);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Ticket>> UpdateTicket(int id, CreateTicketDto dto)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null) return NotFound();

        ticket.Title = dto.Title;
        ticket.Description = dto.Description;
        ticket.Status = dto.Status;
        ticket.Priority = dto.Priority;
        ticket.UpdatedAt = DateTime.UtcNow;

        _context.Entry(ticket).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Tickets.Any(e => e.Id == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTicket(int id)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
        {
            return NotFound();
        }
        _context.Tickets.Remove(ticket);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateTicketStatus(int id, [FromQuery] Status newStatus)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
        {
            return NotFound();
        }

        if (ticket.Status == newStatus)
        {
            return BadRequest("The ticket is already in the specified status.");
        }
        else if (ticket.Status == Status.Open)
        {
            if (newStatus == Status.InProgress)
            {
                ticket.Status = newStatus;
                ticket.UpdatedAt = DateTime.UtcNow;
                _context.Entry(ticket).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            else if (newStatus == Status.Cancelled)
            {
                ticket.Status = newStatus;
                ticket.UpdatedAt = DateTime.UtcNow;
                _context.Entry(ticket).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            else
            {
                return BadRequest("Invalid status transition.");
            }
        }
        else if (ticket.Status == Status.InProgress)
        {
            if (newStatus == Status.Resolved)
            {
                ticket.Status = newStatus;
                ticket.UpdatedAt = DateTime.UtcNow;
                _context.Entry(ticket).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            else if (newStatus == Status.Cancelled)
            {
                ticket.Status = newStatus;
                ticket.UpdatedAt = DateTime.UtcNow;
                _context.Entry(ticket).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            else
            {
                return BadRequest("Invalid status transition.");
            }
        }
        else
        {
            return BadRequest("Invalid status transition.");
        }
    }
}
