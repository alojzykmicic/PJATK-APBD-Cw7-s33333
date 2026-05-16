using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<PC> PCs { get; set; }
    public DbSet<Component> Components { get; set; }
    public DbSet<PCComponent> PcComponents { get; set; }
    public DbSet<ComponentType> ComponentTypes { get; set; }
    public DbSet<ComponentManufacturer> ComponentManufacturers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PC>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Weight).HasColumnType("float");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<ComponentType>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Abbreviation).HasMaxLength(30);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
        });

        modelBuilder.Entity<ComponentManufacturer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Abbreviation).HasMaxLength(30);
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(300);
            entity.Property(e => e.FoundationDate).HasColumnType("date");
        });

        modelBuilder.Entity<Component>(entity =>
        {
            entity.HasKey(e => e.Code);
            entity.Property(e => e.Code).HasColumnType("char(10)");
            entity.Property(e => e.Name).IsRequired().HasMaxLength(300);
            entity.Property(e => e.Description).HasColumnType("nvarchar(max)");

            entity.HasOne(e => e.Manufacturer)
                .WithMany(m => m.Components)
                .HasForeignKey(e => e.ComponentManufacturersID);

            entity.HasOne(e => e.Type)
                .WithMany(t => t.Components)
                .HasForeignKey(e => e.ComponentTypesId);
        });

        modelBuilder.Entity<PCComponent>(entity =>
        {
            entity.HasKey(e => new { e.PCId, e.ComponentCode });
            entity.Property(e => e.ComponentCode).HasColumnType("char(10)");

            entity.HasOne(e => e.PC)
                .WithMany(p => p.PcComponents)
                .HasForeignKey(e => e.PCId);
        });
        
        modelBuilder.Entity<ComponentType>().HasData(
            new ComponentType { Id = 1, Abbreviation = "CPU", Name = "Central Processing Unit" },
            new ComponentType { Id = 2, Abbreviation = "GPU", Name = "Graphics Processing Unit" },
            new ComponentType { Id = 3, Abbreviation = "RAM", Name = "Random Access Memory" }
        );
        
        modelBuilder.Entity<ComponentManufacturer>().HasData(
            new ComponentManufacturer { Id = 1, Abbreviation = "AMD", FullName = "Advanced Micro Devices", FoundationDate = new DateTime(1969, 5, 1) },
            new ComponentManufacturer { Id = 2, Abbreviation = "NV", FullName = "NVIDIA Corporation", FoundationDate = new DateTime(1993, 4, 5) },
            new ComponentManufacturer { Id = 3, Abbreviation = "INT", FullName = "Intel Corporation", FoundationDate = new DateTime(1968, 7, 18) }
        );
        
        modelBuilder.Entity<Component>().HasData(
            new Component { Code = "COMP000001", Name = "Ryzen 5 5600X", Description = "6-core, 12-thread CPU", ComponentManufacturersID = 1, ComponentTypesId = 1 },
            new Component { Code = "COMP000002", Name = "RTX 3060", Description = "12GB GDDR6 GPU", ComponentManufacturersID = 2, ComponentTypesId = 2 },
            new Component { Code = "COMP000003", Name = "Core i5-12400F", Description = "6-core processor", ComponentManufacturersID = 3, ComponentTypesId = 1 }
        );
        
        modelBuilder.Entity<PC>().HasData(
            new PC { Id = 1, Name = "Gaming Beast X", Weight = 12.5, Warranty = 36, CreatedAt = new DateTime(2026, 5, 8, 9, 0, 0), Stock = 5 },
            new PC { Id = 2, Name = "Office Mini Pro", Weight = 4.2, Warranty = 24, CreatedAt = new DateTime(2026, 4, 15, 13, 30, 0), Stock = 12 },
            new PC { Id = 3, Name = "Home Multimedia", Weight = 8.0, Warranty = 24, CreatedAt = new DateTime(2026, 5, 10, 10, 0, 0), Stock = 3 }
        );
        
        modelBuilder.Entity<PCComponent>().HasData(
            new PCComponent { PCId = 1, ComponentCode = "COMP000001", Amount = 1 },
            new PCComponent { PCId = 1, ComponentCode = "COMP000002", Amount = 1 },
            new PCComponent { PCId = 2, ComponentCode = "COMP000003", Amount = 1 }
        );
    }
}