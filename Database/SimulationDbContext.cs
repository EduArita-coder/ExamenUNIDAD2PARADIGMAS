using Microsoft.EntityFrameworkCore;
using ExamenAPI.Entities;

namespace ExamenAPI.Database;

public class SimulationDbContext : DbContext
{
    public SimulationDbContext(DbContextOptions<SimulationDbContext> options) : base(options)
    {
    }

    public DbSet<SimulationEntity> Simulations => Set<SimulationEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SimulationEntity>(entity =>
        {
            entity.ToTable("Simulations");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DepositoInicial).IsRequired();
            entity.Property(e => e.TasaInteresAnual).IsRequired();
            entity.Property(e => e.PlazoEnAños).IsRequired();
            entity.Property(e => e.BalanceFinal);
            entity.Property(e => e.InteresTotal);
            entity.Property(e => e.FechaCreacion).IsRequired();
        });
    }
}