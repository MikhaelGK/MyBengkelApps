using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BENGKEL_API.Models;

public partial class SidorejoBengkelContext : DbContext
{
    public SidorejoBengkelContext()
    {
    }

    public SidorejoBengkelContext(DbContextOptions<SidorejoBengkelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerVehicle> CustomerVehicles { get; set; }

    public virtual DbSet<DetailTrx> DetailTrxes { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<HeaderTrx> HeaderTrxes { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=SIDOREJO_BENGKEL;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.Property(e => e.CustomerId)
                .HasMaxLength(6)
                .IsFixedLength();
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CustomerVehicle>(entity =>
        {
            entity.ToTable("CustomerVehicle");

            entity.Property(e => e.CustomerId)
                .HasMaxLength(6)
                .IsFixedLength();
            entity.Property(e => e.Number)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.VehicleId)
                .HasMaxLength(6)
                .IsFixedLength();

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerVehicles)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerVehicle_Customer");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.CustomerVehicles)
                .HasForeignKey(d => d.VehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerVehicle_Vehicle");
        });

        modelBuilder.Entity<DetailTrx>(entity =>
        {
            entity.ToTable("DetailTrx");

            entity.Property(e => e.TrxId)
                .HasMaxLength(10)
                .IsFixedLength();

            entity.HasOne(d => d.CustomerVehicle).WithMany(p => p.DetailTrxes)
                .HasForeignKey(d => d.CustomerVehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetailTrx_CustomerVehicle");

            entity.HasOne(d => d.Trx).WithMany(p => p.DetailTrxes)
                .HasForeignKey(d => d.TrxId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetailTrx_HeaderTrx");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employee");

            entity.Property(e => e.EmployeeId)
                .HasMaxLength(6)
                .IsFixedLength();
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Position)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HeaderTrx>(entity =>
        {
            entity.HasKey(e => e.TrxId);

            entity.ToTable("HeaderTrx");

            entity.Property(e => e.TrxId)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.CustomerId)
                .HasMaxLength(6)
                .IsFixedLength();
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(6)
                .IsFixedLength();

            entity.HasOne(d => d.Customer).WithMany(p => p.HeaderTrxes)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HeaderTrx_Customer");

            entity.HasOne(d => d.Employee).WithMany(p => p.HeaderTrxes)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HeaderTrx_Employee");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.ToTable("Vehicle");

            entity.Property(e => e.VehicleId)
                .HasMaxLength(6)
                .IsFixedLength();
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
