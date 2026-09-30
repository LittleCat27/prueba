using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace apiprueba.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<LoginLog> LoginLogs { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlite("Data Source=C:\\sqlite\\prueba.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LoginLog>(entity =>
        {
            entity.ToTable("login_log");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("INTEGER")
                .HasColumnName("id");
            entity.Property(e => e.Fecha)
                .HasColumnType("timestamp")
                .HasColumnName("fecha");
            entity.Property(e => e.Success)
                .HasColumnType("bool")
                .HasColumnName("success");
            entity.Property(e => e.UsuarioId)
                .HasColumnType("INT")
                .HasColumnName("usuario_id");

            entity.HasOne(d => d.Usuario).WithMany(p => p.LoginLogs).HasForeignKey(d => d.UsuarioId);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("usuario");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("INTEGER")
                .HasColumnName("id");
            entity.Property(e => e.IsActive)
                .HasColumnType("bool")
                .HasColumnName("isActive");
            entity.Property(e => e.Mail)
                .HasColumnType("varchar(255)")
                .HasColumnName("mail");
            entity.Property(e => e.Password)
                .HasColumnType("varchar(255)")
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasColumnType("varchar(255)")
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
