﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace P0_brendan_BankingApp.POCO;

public partial class P0BrendanBankingDbContext : DbContext
{
    public P0BrendanBankingDbContext()
    {
    }

    public P0BrendanBankingDbContext(DbContextOptions<P0BrendanBankingDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=P0_brendan_BankingDB;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccId).HasName("PK__Account__91CBC378EC92765C");

            entity.ToTable("Account");

            entity.Property(e => e.AccType)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Balance).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Customer).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Customer");
        });

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admin__719FE4883A40FC9D");

            entity.ToTable("Admin");

            entity.Property(e => e.AdminUsername)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Salt).HasMaxLength(16);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64D8EEC507E3");

            entity.ToTable("Customer");

            entity.Property(e => e.CustomerUsername)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Salt).HasMaxLength(16);
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__Request__33A8517A2B9C99D7");

            entity.ToTable("Request");

            entity.Property(e => e.RequestDate).HasColumnType("datetime");
            entity.Property(e => e.RequestType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Acc).WithMany(p => p.Requests)
                .HasForeignKey(d => d.AccId)
                .HasConstraintName("FK_AccountRequest");

            entity.HasOne(d => d.Admin).WithMany(p => p.Requests)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AdminRequest");

            entity.HasOne(d => d.Customer).WithMany(p => p.Requests)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerRequest");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
