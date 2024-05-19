using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Zadanie5.Models;

namespace Zadanie5.Context;

public partial class ApbdContext : DbContext
{
    public ApbdContext()
    {
    }

    public ApbdContext(DbContextOptions<ApbdContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientTrip> ClientTrips { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=apbd;Username=postgres;Password=admin;Port=5432")
            .LogTo(Console.WriteLine, LogLevel.Information);
        
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Idclient).HasName("client_pk");

            entity.ToTable("client", "trip");

            entity.Property(e => e.Idclient)
                .ValueGeneratedNever()
                .HasColumnName("idclient");
            entity.Property(e => e.Email)
                .HasMaxLength(120)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(120)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(120)
                .HasColumnName("lastname");
            entity.Property(e => e.Pesel)
                .HasMaxLength(120)
                .HasColumnName("pesel");
            entity.Property(e => e.Telephone)
                .HasMaxLength(120)
                .HasColumnName("telephone");
        });

        modelBuilder.Entity<ClientTrip>(entity =>
        {
            entity.HasKey(e => new { e.Idclient, e.Idtrip }).HasName("client_trip_pk");

            entity.ToTable("client_trip", "trip");

            entity.Property(e => e.Idclient).HasColumnName("idclient");
            entity.Property(e => e.Idtrip).HasColumnName("idtrip");
            entity.Property(e => e.Paymentdate).HasColumnName("paymentdate");
            entity.Property(e => e.Registeredat).HasColumnName("registeredat");

            entity.HasOne(d => d.IdclientNavigation).WithMany(p => p.ClientTrips)
                .HasForeignKey(d => d.Idclient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("table_5_client");

            entity.HasOne(d => d.IdtripNavigation).WithMany(p => p.ClientTrips)
                .HasForeignKey(d => d.Idtrip)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("table_5_trip");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Idcountry).HasName("country_pk");

            entity.ToTable("country", "trip");

            entity.Property(e => e.Idcountry)
                .ValueGeneratedNever()
                .HasColumnName("idcountry");
            entity.Property(e => e.Name)
                .HasMaxLength(120)
                .HasColumnName("name");

            entity.HasMany(d => d.Idtrips).WithMany(p => p.Idcountries)
                .UsingEntity<Dictionary<string, object>>(
                    "CountryTrip",
                    r => r.HasOne<Trip>().WithMany()
                        .HasForeignKey("Idtrip")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("country_trip_trip"),
                    l => l.HasOne<Country>().WithMany()
                        .HasForeignKey("Idcountry")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("country_trip_country"),
                    j =>
                    {
                        j.HasKey("Idcountry", "Idtrip").HasName("country_trip_pk");
                        j.ToTable("country_trip", "trip");
                        j.IndexerProperty<int>("Idcountry").HasColumnName("idcountry");
                        j.IndexerProperty<int>("Idtrip").HasColumnName("idtrip");
                    });
        });

        modelBuilder.Entity<Trip>(entity =>
        {
            entity.HasKey(e => e.Idtrip).HasName("trip_pk");

            entity.ToTable("trip", "trip");

            entity.Property(e => e.Idtrip)
                .ValueGeneratedNever()
                .HasColumnName("idtrip");
            entity.Property(e => e.Datefrom).HasColumnName("datefrom");
            entity.Property(e => e.Dateto).HasColumnName("dateto");
            entity.Property(e => e.Description)
                .HasMaxLength(220)
                .HasColumnName("description");
            entity.Property(e => e.Maxpeople).HasColumnName("maxpeople");
            entity.Property(e => e.Name)
                .HasMaxLength(120)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
