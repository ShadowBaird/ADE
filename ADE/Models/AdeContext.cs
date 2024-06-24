using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ADE.Models;

public partial class AdeContext : DbContext
{
    public AdeContext()
    {
    }

    public AdeContext(DbContextOptions<AdeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrador> Administradores { get; set; }

    public virtual DbSet<Evento> Eventos { get; set; }

    public virtual DbSet<Prereserva> Prereservas { get; set; }

    public virtual DbSet<Salon> Salones { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = configuration.GetConnectionString("AdeConnection");

            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrador>(entity =>
        {
            entity.HasKey(e => e.IdAdmin);

            entity.Property(e => e.IdAdmin).HasColumnName("id_admin");
            entity.Property(e => e.ContraAdmin)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("contra_admin");
            entity.Property(e => e.IdSalon).HasColumnName("id_salon");
            entity.Property(e => e.UsuarioAdmin)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("usuario_admin");

            entity.HasOne(d => d.IdSalonNavigation).WithMany(p => p.Administradores)
                .HasForeignKey(d => d.IdSalon)
                .HasConstraintName("FK_Administradores_Salones");
        });

        modelBuilder.Entity<Evento>(entity =>
        {
            entity.HasKey(e => e.IdEvento);

            entity.HasIndex(e => new { e.IdSalon, e.FechaEvento }, "index1").IsUnique();

            entity.Property(e => e.IdEvento).HasColumnName("id_evento");
            entity.Property(e => e.AbonadoEvento).HasColumnName("abonado_evento");
            entity.Property(e => e.CantidadPersonasEvento).HasColumnName("cantidad_personas_evento");
            entity.Property(e => e.FechaEvento).HasColumnName("fecha_evento");
            entity.Property(e => e.IdPrereserva).HasColumnName("id_prereserva");
            entity.Property(e => e.IdSalon).HasColumnName("id_salon");
            entity.Property(e => e.NombreResponsableEvento)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("nombre_responsable_evento");
            entity.Property(e => e.TelefonoResponsableEvento)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("telefono_responsable_evento");
            entity.Property(e => e.TipoEvento)
                .HasMaxLength(30)
                .IsFixedLength()
                .HasColumnName("tipo_evento");
            entity.Property(e => e.TotalPagarEvento).HasColumnName("total_pagar_evento");

            entity.HasOne(d => d.IdPrereservaNavigation).WithMany(p => p.Eventos)
                .HasForeignKey(d => d.IdPrereserva)
                .HasConstraintName("FK_Eventos_Prereservas");

            entity.HasOne(d => d.IdSalonNavigation).WithMany(p => p.Eventos)
                .HasForeignKey(d => d.IdSalon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Eventos_Salones");
        });

        modelBuilder.Entity<Prereserva>(entity =>
        {
            entity.HasKey(e => e.IdPrereserva);

            entity.Property(e => e.IdPrereserva).HasColumnName("id_prereserva");
            entity.Property(e => e.CantidadPersonasPrereserva).HasColumnName("cantidad_personas_prereserva");
            entity.Property(e => e.EstadoPrereserva)
                .HasMaxLength(30)
                .IsFixedLength()
                .HasColumnName("estado_prereserva");
            entity.Property(e => e.FechaEventoPrereserva).HasColumnName("fecha_evento_prereserva");
            entity.Property(e => e.FechaRegistroPrereserva).HasColumnName("fecha_registro_prereserva");
            entity.Property(e => e.IdSalon).HasColumnName("id_salon");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.TipoEventoPrereserva)
                .HasMaxLength(30)
                .IsFixedLength()
                .HasColumnName("tipo_evento_prereserva");
            entity.Property(e => e.TotalPrereserva)
                .HasDefaultValue(0)
                .HasColumnName("total_prereserva");

            entity.HasOne(d => d.IdSalonNavigation).WithMany(p => p.Prereservas)
                .HasForeignKey(d => d.IdSalon)
                .HasConstraintName("FK_Prereservas_Salones");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Prereservas)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Prereservas_Usuarios");
        });

        modelBuilder.Entity<Salon>(entity =>
        {
            entity.HasKey(e => e.IdSalon);

            entity.Property(e => e.IdSalon).HasColumnName("id_salon");
            entity.Property(e => e.NomSalon)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("nom_salon");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);

            entity.HasIndex(e => e.CorreoUsuario, "IX_Usuarios").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.ContraUsuario)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("contra_usuario");
            entity.Property(e => e.CorreoUsuario)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("correo_usuario");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("nombre_usuario");
            entity.Property(e => e.TelUsuario)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("tel_usuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
