
using Microsoft.EntityFrameworkCore;
using Ticket.Api.Models;

namespace Ticket.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Event> Events => Set<Event>();

    public DbSet<User> Users => Set<User>();

    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Event>()
            .Property(e => e.Name)
            .HasMaxLength(200)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .HasMaxLength(255)
            .IsRequired();

        modelBuilder.Entity<Booking>()
            .Property(b => b.Status)
            .HasMaxLength(20)
            .HasDefaultValue("Pending");

        modelBuilder.Entity<Booking>()
            .HasOne<Event>()
            .WithMany()
            .HasForeignKey(b => b.EventId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Booking>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
