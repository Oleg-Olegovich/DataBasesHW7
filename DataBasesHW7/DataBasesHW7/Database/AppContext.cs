using DataBasesHW7.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBasesHW7.Database;

public sealed class AppContext : DbContext
{
    public DbSet<Sportsman>? Sportsmen { get; set; }
    public DbSet<Country>? Countries { get; set; }
    public DbSet<Olympiad>? Olympiads { get; set; }
    public DbSet<Event>? Events { get; set; }
    public DbSet<Result>? Results { get; set; }

    public AppContext()
        => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Database=my_postgres;Username=postgres;Password=admin");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Sportsman>().HasKey(s => s.Id);
        modelBuilder.Entity<Country>().HasKey(c => c.Id);
        modelBuilder.Entity<Olympiad>().HasKey(o => o.Id);
        modelBuilder.Entity<Event>().HasKey(e => e.Id);
        modelBuilder.Entity<Result>().HasKey(r => r.Id);

        modelBuilder.Entity<Sportsman>()
            .HasMany(s => s.Results)
            .WithOne(r => r.Sportsman)
            .IsRequired();
        modelBuilder.Entity<Country>()
            .HasMany(c => c.Olympiads)
            .WithOne(o => o.Country)
            .IsRequired();
        modelBuilder.Entity<Event>()
            .HasMany(e => e.Results)
            .WithOne(r => r.Event)
            .IsRequired();
        modelBuilder.Entity<Olympiad>()
            .HasMany(o => o.Events)
            .WithOne(e => e.Olympiad)
            .IsRequired();

        base.OnModelCreating(modelBuilder);
    }
}