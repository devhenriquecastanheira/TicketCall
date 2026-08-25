using Microsoft.EntityFrameworkCore;
using TicketCall.Api.Entities;

namespace TicketCall.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Ticket> Tickets { get; set; }
}
