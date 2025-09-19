using System;
using System.Collections.Generic;
using FoodDelivery.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Domain.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<DeliveryAgent> DeliveryAgents { get; set; }

    public virtual DbSet<MenuItem> MenuItems { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Restaurant> Restaurants { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server= localhost;Initial Catalog= FoodDeliverySystem; User=SA; Password=password-1 ; TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__Addresse__091C2AFB40E5AA85");

            entity.Property(e => e.IsDefault).HasDefaultValue(false);

            entity.HasOne(d => d.User).WithMany(p => p.Addresses).HasConstraintName("FK__Addresses__UserI__440B1D61");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Cart__51BCD7B7199B0F44");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.Carts).HasConstraintName("FK__Cart__Restaurant__4CA06362");

            entity.HasOne(d => d.User).WithMany(p => p.Carts).HasConstraintName("FK__Cart__UserId__4BAC3F29");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.CartItemId).HasName("PK__CartItem__488B0B0A2CA0117A");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems).HasConstraintName("FK__CartItems__CartI__4F7CD00D");

            entity.HasOne(d => d.Item).WithMany(p => p.CartItems).HasConstraintName("FK__CartItems__ItemI__5070F446");
        });

        modelBuilder.Entity<DeliveryAgent>(entity =>
        {
            entity.HasKey(e => e.AgentId).HasName("PK__Delivery__9AC3BFF11F17B8BC");

            entity.Property(e => e.IsAvailable).HasDefaultValue(true);

            entity.HasOne(d => d.User).WithMany(p => p.DeliveryAgents).HasConstraintName("FK__DeliveryA__UserI__3D5E1FD2");
        });

        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__MenuItem__727E838B72A58726");

            entity.Property(e => e.IsAvailable).HasDefaultValue(true);

            entity.HasOne(d => d.Restaurant).WithMany(p => p.MenuItems).HasConstraintName("FK__MenuItems__Resta__47DBAE45");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCFA45282F6");

            entity.Property(e => e.OrderDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Address).WithMany(p => p.Orders).HasConstraintName("FK__Orders__AddressI__5629CD9C");

            entity.HasOne(d => d.Agent).WithMany(p => p.Orders).HasConstraintName("FK__Orders__AgentId__5535A963");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.Orders).HasConstraintName("FK__Orders__Restaura__5441852A");

            entity.HasOne(d => d.User).WithMany(p => p.Orders).HasConstraintName("FK__Orders__UserId__534D60F1");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PK__OrderIte__57ED06811D1ABAC9");

            entity.HasOne(d => d.Item).WithMany(p => p.OrderItems).HasConstraintName("FK__OrderItem__ItemI__5AEE82B9");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems).HasConstraintName("FK__OrderItem__Order__59FA5E80");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A383E80F80E");

            entity.Property(e => e.PaymentDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments).HasConstraintName("FK__Payments__OrderI__5DCAEF64");
        });

        modelBuilder.Entity<Restaurant>(entity =>
        {
            entity.HasKey(e => e.RestaurantId).HasName("PK__Restaura__87454C95F8BEA82A");

            entity.HasOne(d => d.User).WithMany(p => p.Restaurants).HasConstraintName("FK__Restauran__UserI__412EB0B6");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C05CB10B6");

            entity.Property(e => e.IsVerified).HasDefaultValue(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
